using System.Threading.Tasks;

namespace InfoTrack.Assignment.Domain
{
    public interface ISearchStrategy
    {
        Task<SearchResult> Search(SearchContext context);
    }
}