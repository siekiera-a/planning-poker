using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Client.Models
{
    public interface IApiClient
    {
        Task<Response<T>> PostAsync<T>(string url, object o);
        Task<Response<T>> PostAsyncAuth<T>(string url, object o);
        Task<Response<T>> GetAsyncAuth<T>(string url);
    }

    public class ApiClient : IApiClient
    {
        private readonly IHttpClientFactory _client;
        private readonly ITokenManager _token;

        public ApiClient(IHttpClientFactory factory, ITokenManager token)
        {
            _client = factory;
            _token = token;
        }

        public async Task<Response<T>> PostAsyncAuth<T>(string url, object o)
        {
            using var httpClient = _client.CreateAuthenticated(_token.Token);
            return await SendPostAsync<T>(url, o, httpClient);
        }

        public async Task<Response<T>> GetAsyncAuth<T>(string url)
        {
            using var httpClient = _client.CreateAuthenticated(_token.Token);
            return await SendGetAsync<T>(url, httpClient);
        }

        public async Task<Response<T>> PostAsync<T>(string url, object o)
        {
            using var httpClient = _client.Create();
            return await SendPostAsync<T>(url, o, httpClient);
        }

        private async Task<Response<T>> SendPostAsync<T>(string url, object o, HttpClient httpClient)
        {
            var response = await httpClient.PostAsync(_client.Path(url), GetContent(o));
            return await GetResponse<T>(response);
        }

        private async Task<Response<T>> SendGetAsync<T>(string url, HttpClient httpClient)
        {
            var response = await httpClient.GetAsync(_client.Path(url));
            return await GetResponse<T>(response);
        }


        private async Task<Response<T>> GetResponse<T>(HttpResponseMessage response)
        {
            var myResponse = new Response<T>(response);
            await myResponse.ParseContentAsync();
            return myResponse;
        }

        private HttpContent GetContent(object o)
        {
            var json = JsonConvert.SerializeObject(o);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}