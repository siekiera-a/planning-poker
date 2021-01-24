using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models
{
    public interface ITokenManager
    {
        string Token { get; set; }
        void RemoveToken();
    }

    public class TokenManager : ITokenManager
    {
        public string Token { get; set; }

        public void RemoveToken()
        {
            Token = "";
        }
    }
}