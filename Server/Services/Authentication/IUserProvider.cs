using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.Utils;

namespace Server.Services.Authentication
{
    public interface IUserProvider
    {
        int GetUserId();
    }
}
