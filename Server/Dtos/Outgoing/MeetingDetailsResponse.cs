using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.Models.Dapper;

namespace Server.Dtos.Outgoing
{
    public class MeetingDetailsResponse : MeetingDetails
    {

        public bool CanJoin { get; set; }

    }
}
