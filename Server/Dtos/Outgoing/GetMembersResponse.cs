using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.Models.Dapper;

namespace Server.Dtos.Outgoing
{
    public class GetMembersResponse : TeamIdResponse
    {

        public List<User> Members { get; set; }

    }
}
