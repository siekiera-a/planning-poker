using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models
{
    public interface IUserDataProvider
    {
        string Username { get; set; }
        string Email { get; set; }
    }

    public class UserDataProvider : IUserDataProvider
    {
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
