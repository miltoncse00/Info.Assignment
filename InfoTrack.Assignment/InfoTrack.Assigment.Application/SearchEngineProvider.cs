using InfoTrack.Assignment.Domain;

namespace InfoTrack.Assignment.Application
{
    public class SearchEngineProvider: ISearchEngineProvider
    {
        public ISearchStrategy GetSearchStrategy(SearchEngineType searchEngineType, string searchEngineUrl)
        {
            switch (searchEngineType)
            {
                case SearchEngineType.InfoTrack:
                    return new InfoTrackSearchStrategy(searchEngineUrl);
                default:
                    return new InfoTrackSearchStrategy(searchEngineUrl);
            }
        }
    }
}
