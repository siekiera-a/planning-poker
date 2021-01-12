using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace Server.Services.Authentication
{
    public interface IJwtTokenManager
    {
        string CreateToken(int userId);
    }
}
