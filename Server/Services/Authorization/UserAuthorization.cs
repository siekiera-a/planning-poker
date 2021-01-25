using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.DAOs;
using Server.Services.Meeting;
using TeamAction = Server.Services.Team.TeamAction;

namespace Server.Services.Authorization
{
    public class UserAuthorization : IUserAuthorization
    {

        private readonly RolesDAO _rolesDao;
        private readonly MeetingDAO _meetingDao;

        private readonly string _adminRole;
        private readonly string _memberRole;
        private readonly string _moderatorRole;

        public UserAuthorization(RolesDAO rolesDao, MeetingDAO meetingDao)
        {
            _rolesDao = rolesDao;
            _meetingDao = meetingDao;

            _adminRole = "ADMIN";
            _memberRole = "MEMBER";
            _moderatorRole = "MODERATOR";
        }


        public async Task<bool> Authorize(int userId, int teamId, TeamAction action)
        {
            List<string> userRoles = await _rolesDao.GetRoles(userId, teamId);
            List<string> acceptedRoles;

            switch (action)
            {
                case TeamAction.RemoveTeam:
                case TeamAction.GenerateJoinCode:
                case TeamAction.ChangeRole:
                case TeamAction.RenameTeam:
                case TeamAction.RemoveJoinCode:
                case TeamAction.RemoveMember:
                    return userRoles.Contains(_adminRole);

                case TeamAction.AddMember:
                case TeamAction.CreateMeeting:
                    acceptedRoles = new List<string> { _adminRole, _moderatorRole };
                    return userRoles.Intersect(acceptedRoles).Any();


                case TeamAction.MemberAccess:

                    acceptedRoles = new List<string> { _adminRole, _moderatorRole, _memberRole, };
                    return userRoles.Intersect(acceptedRoles).Any();

            }

            return false;
        }

        public async Task<bool> Authorize(int userId, int meetingId, MeetingAction action)
        {
            var isOrganizer = await _meetingDao.IsTheMeetingOrganizer(userId, meetingId);

            switch (action)
            {
                case MeetingAction.AssignUser:
                case MeetingAction.InviteUser:
                case MeetingAction.RemoveInvitation:
                case MeetingAction.AddTask:
                    return isOrganizer;

                case MeetingAction.RescheduleMeeting:
                case MeetingAction.RemoveMeeting:
                    return isOrganizer;
            }

            return false;
        }
    }
}
