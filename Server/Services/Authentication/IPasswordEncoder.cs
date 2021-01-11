using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services.Authentication
{
    public interface IPasswordEncoder
    {
        string HashPassword(string password);
        bool Verify(string password, string hash);
    }
}
