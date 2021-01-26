using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Server.Services.Authentication
{
    public class UserProvider : IUserProvider
    {

        private readonly IHttpContextAccessor _context;

        public UserProvider(IHttpContextAccessor context)
        {
            _context = context;
        }

        public int GetUserId()
        {
            var id = _context.HttpContext.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value;
            return int.Parse(id);
        }
    }
}
