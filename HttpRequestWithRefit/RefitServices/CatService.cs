using Refit;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpRequestWithRefit.RefitServices
{
    public class CatService
    {
        private readonly ICatApi _catApi;
        public CatService()
        {
            const string baseUrl = "https://cat-fact.herokuapp.com";
            var httpClient = new HttpClient(new CatLoggingHandler()) { BaseAddress = new Uri(baseUrl) };
            _catApi = RestService.For<ICatApi>(httpClient);
        }

        public async Task<string> GetFacts()
        {
            return await _catApi.GetFacts();
        }

        public async Task<string> GetFact(string id)
        {
            return await _catApi.GetFact(id);
        }
    }
}
