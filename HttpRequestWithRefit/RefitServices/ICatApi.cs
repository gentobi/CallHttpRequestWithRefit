using Refit;
using System.Threading.Tasks;
namespace HttpRequestWithRefit.RefitServices
{
    public interface ICatApi
    {
        [Get("/facts")]
        Task<string> GetFacts();

        [Get("/fatct?id={id}")]
        Task<string> GetFact(string id);
    }
}
