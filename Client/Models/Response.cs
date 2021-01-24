using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Client.Models
{
    public class Response<T>
    {
        public HttpStatusCode HttpStatusCode { get; }
        public bool IsOk { get; }

        public Error Error { get; private set; }

        private readonly HttpResponseMessage _responseMessage;

        public async Task ParseContentAsync()
        {
            var content = await _responseMessage.Content.ReadAsStringAsync();
            if (IsOk)
            {
                Value = JsonConvert.DeserializeObject<T>(content);
            }
            else
            {
                Error = JsonConvert.DeserializeObject<Error>(content);
            }
        }

        public T Value { get; private set; }

        public Response(HttpResponseMessage responseMessage)
        {
            _responseMessage = responseMessage;
            HttpStatusCode = responseMessage.StatusCode;
            IsOk = responseMessage.IsSuccessStatusCode;
        }
    }
}