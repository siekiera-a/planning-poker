using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.Models.Dapper;
using Server.Utils;

namespace Server.Services.Authentication
{
    public interface IUserService
    {

        Task<Optional<User>> Register(string username, string email, string password);
        Task<Optional<User>> Login(string email, string password);
        Task<Optional<User>> Login(string refreshToken);
        Task Logout(int id);

    }
}
