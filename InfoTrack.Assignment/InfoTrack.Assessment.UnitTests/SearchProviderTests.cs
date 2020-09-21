using System;
using System.IO;
using System.Reflection;
using InfoTrack.Assignment.Application;
using InfoTrack.Assignment.Domain;
using Xunit;

namespace InfoTrack.Assessment.UnitTests
{
    public class SearchProviderTests
    {
        [Fact]
        public void GivenTheProviderSearchWithType()
        {
            var searchProvider  = new SearchEngineProvider();
            var searchEngine = searchProvider.GetSearchStrategy(SearchEngineType.InfoTrack, string.Empty);
            Assert.IsType<InfoTrackSearchStrategy>(searchEngine);
        }
    }
}
