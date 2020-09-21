namespace InfoTrack.Assignment.Domain
{
    public interface ISearchEngineProvider
    {
        ISearchStrategy GetSearchStrategy (SearchEngineType searchEngineType, string searchEngineUrl);
    }
}