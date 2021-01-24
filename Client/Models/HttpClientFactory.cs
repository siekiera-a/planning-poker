using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace Client.Models
{
    public interface IHttpClientFactory
    {
        HttpClient CreateAuthenticated(string token);
        HttpClient Create();
        string Path(string endpoint);
    }

    public class HttpClientFactory : IHttpClientFactory
    {
        private readonly Uri _baseAddress;

        public HttpClientFactory()
        {
            _baseAddress = new Uri("http://192.168.0.73:5000/api");
        }

        public HttpClient CreateAuthenticated(string token)
        {
            var client = Create();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }

        public HttpClient Create()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = _baseAddress;
            return client;
        }

        public string Path(string endpoint)
        {
            return _baseAddress.AbsolutePath + endpoint;
        }
    }
}