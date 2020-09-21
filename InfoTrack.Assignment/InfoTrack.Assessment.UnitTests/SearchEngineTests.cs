using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using InfoTrack.Assignment.Application;
using InfoTrack.Assignment.Domain;
using NSubstitute;
using NSubstitute.Extensions;
using Xunit;

namespace InfoTrack.Assessment.UnitTests
{
    public class SearchEngineTests
    {
        [Fact]
        public async Task GivenTheServiceAllPageReturningValueVerifyTheValueForInfoTrack()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"test.html");
            string htmlString = File.ReadAllText(path);
            var inforTrack = Substitute.ForPartsOf<InfoTrackSearchStrategy>(string.Empty);
            inforTrack.Configure().GetType().GetMethod("DownloadPageHtml", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.Invoke(inforTrack, new object[1]{Arg.Any<int>()}).Returns(Task.FromResult(htmlString));
            var result = await inforTrack.Search(new SearchContext()
                {MaxSearchResult = 50, SearchInput = new SearchInput {site = "infotrack.com.au"}});
            Assert.Equal("1,11,21,31,41",result.Positions);
        }

        [Fact]
        public async Task GivenTheServiceAllPageReturningValueVerifyTheValueForUnkownUrl()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"test.html");
            string htmlString = File.ReadAllText(path);
            var inforTrack = Substitute.ForPartsOf<InfoTrackSearchStrategy>(string.Empty);
            inforTrack.Configure().GetType().GetMethod("DownloadPageHtml", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.Invoke(inforTrack, new object[1] { Arg.Any<int>() }).Returns(Task.FromResult(htmlString));
            var result = await inforTrack.Search(new SearchContext()
                { MaxSearchResult = 50, SearchInput = new SearchInput { site = "unknown.com.au" } });
            Assert.Equal("0", result.Positions);
        }

        [Fact]
        public async Task GivenTheServiceAllPageReturningValueVerifyTheValueForMaxSize20()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"test.html");
            string htmlString = File.ReadAllText(path);
            var inforTrack = Substitute.ForPartsOf<InfoTrackSearchStrategy>(string.Empty);
            inforTrack.Configure().GetType().GetMethod("DownloadPageHtml", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.Invoke(inforTrack, new object[1] { Arg.Any<int>() }).Returns(Task.FromResult(htmlString));
            var result = await inforTrack.Search(new SearchContext()
            { MaxSearchResult = 20, SearchInput = new SearchInput { site = "infotrack.com.au" } });
            Assert.Equal("1,11", result.Positions);
        }
    }
}
