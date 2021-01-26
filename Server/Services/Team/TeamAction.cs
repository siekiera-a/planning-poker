using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services.Team
{
    public enum TeamAction
    {
        RemoveTeam,
        AddMember,
        RemoveMember,
        ChangeRole,
        GenerateJoinCode,
        RemoveJoinCode,
        RenameTeam,
        CreateMeeting,
        MemberAccess
    }
}
