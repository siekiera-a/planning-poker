using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services.Authorization
{
    public interface IUserAuthorization
    {

        public Task<bool> Authorize(int userId, int teamId, Team.Action action);

    }
}
