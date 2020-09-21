using InfoTrack.Assignment.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace InfoTrack.Assignment.Application
{
    public class InfoTrackSearchStrategy : ISearchStrategy
    {
        private readonly string _searchEngineUrl;
        private int _recordInEachPage = 10;

        public InfoTrackSearchStrategy(string searchEngineUrl)
        {
            _searchEngineUrl = searchEngineUrl;
        }

        public async Task<SearchResult> Search(SearchContext context)
        {
            var totalPagesToSearch = (context.MaxSearchResult + _recordInEachPage - 1) / _recordInEachPage;
            var taskList = new List<Task<List<int>>>();
            for (int pageNo = 1; pageNo <= totalPagesToSearch; pageNo++)
            {
                taskList.Add(SearchPage(context, pageNo));
            }

            var results = await Task.WhenAll(taskList);

            var result = results.SelectMany(s => s).ToList();

            if (result.Count == 0)
                result.Add(0);

            var searchResult = new SearchResult { Positions = string.Join(",", result), SearchEngineType = SearchEngineType.InfoTrack.ToString()};

            return searchResult;
        }

        protected virtual async Task<List<int>> SearchPage(SearchContext context, int pageNo)
        {
            var htmlString = await DownloadPageHtml(pageNo);

            var result = GetLinkCountFromResponse(context, pageNo, htmlString);

            return result;
        }

        protected virtual async Task<string> DownloadPageHtml(int pageNo)
        {
            var searchPage = string.Format(_searchEngineUrl, pageNo.ToString().PadLeft(2, '0'));
            using var webClient = new WebClient();
            var htmlString = await webClient.DownloadStringTaskAsync(searchPage);
            return htmlString;
        }

        protected virtual List<int> GetLinkCountFromResponse(SearchContext context, int pageNo, string htmlString)
        {
            var index = 1;
            var result = new List<int>();
            var siteLinkPattern = new Regex(@"<div[ ]*class=""r""><a[ ]href=""(.*?)""[^>]*>.*?<[/]a>", RegexOptions.Singleline);

            var match = siteLinkPattern.Matches(htmlString);
            foreach (Match m in match)
            {
                var isMatch = m.ToString().Contains(context.SearchInput.site);

                if (isMatch)
                {
                    result.Add(index + (pageNo - 1) * _recordInEachPage);
                }

                index++;
            }

            return result;
        }
    }
}
