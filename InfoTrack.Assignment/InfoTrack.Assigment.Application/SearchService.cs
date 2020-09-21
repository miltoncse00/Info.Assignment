using System.Collections.Generic;
using System.Threading.Tasks;
using InfoTrack.Assignment.Domain;

namespace InfoTrack.Assignment.Application
{
    public interface ISearchService
    {
        Task<List<SearchResult>> Search(SearchInput searchInput);
    }

    public class SearchService : ISearchService
    {
        private readonly SearchConfiguration _searchConfiguration;
        private readonly ISearchEngineProvider _provider;

        public SearchService(SearchConfiguration searchConfiguration, ISearchEngineProvider provider)
        {
            _searchConfiguration = searchConfiguration;
            _provider = provider;
        }

        public async Task<List<SearchResult>> Search(SearchInput searchInput)
        {
            if(searchInput == null || string.IsNullOrWhiteSpace(searchInput.site))
                throw new ValidationException("Invalid input.User must provide site value for search");

            var searchResults = new List<SearchResult>();
            var searchStrategy =
                _provider.GetSearchStrategy(SearchEngineType.InfoTrack, _searchConfiguration.InfoTrackSearchUrl);

            var infoTrackSearchResult = await searchStrategy.Search(new SearchContext
            { SearchInput = searchInput, MaxSearchResult = _searchConfiguration.MaxSearchResult });

            searchResults.Add(infoTrackSearchResult);

            return searchResults;
        }
    }
}
