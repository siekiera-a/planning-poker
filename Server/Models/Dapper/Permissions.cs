using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models.Dapper
{
    public class Permissions
    {

        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsOrganizer { get; set; }
        public bool IsInvited { get; set; }

    }
}
