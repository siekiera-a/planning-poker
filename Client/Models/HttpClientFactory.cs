using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace Client.Models
{
    public class HttpClientFactory
    {
        private static HttpClient _client;
        private static readonly string BaseUrl;

        static HttpClientFactory()
        {
            _client = new HttpClient();
            BaseUrl = "https://localhost:5001/";
        }

        public static HttpClient Authorized(string url, string token)
        {
            _client = UnAuthorized(url);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return _client;
        }

        public static HttpClient UnAuthorized(string url)
        {
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.BaseAddress = new Uri(BaseUrl + url);

            return _client;
        }

}
}
