using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services.Meeting
{
    public enum MeetingAction
    {
        AssignUser,
        InviteUser,
        RemoveInvitation,
        RescheduleMeeting,
        RemoveMeeting,
        AddTask
    }
}
