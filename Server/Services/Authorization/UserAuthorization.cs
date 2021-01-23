using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.DAOs;
using Action = Server.Services.Team.Action;

namespace Server.Services.Authorization
{
    public class UserAuthorization : IUserAuthorization
    {

        private readonly RolesDAO _rolesDao;

        private readonly string _adminRole;
        private readonly string _memberRole;
        private readonly string _moderatorRole;

        public UserAuthorization(RolesDAO rolesDao)
        {
            _rolesDao = rolesDao;

            _adminRole = "ADMIN";
            _memberRole = "MEMBER";
            _moderatorRole = "MODERATOR";
        }


        public async Task<bool> Authorize(int userId, int teamId, Action action)
        {

            List<string> userRoles = await _rolesDao.GetRoles(userId, teamId);
            List<string> acceptedRoles;

            switch (action)
            {
                case Action.RemoveTeam:
                case Action.GenerateJoinCode:
                case Action.ChangeRole:
                case Action.RenameTeam:
                case Action.RemoveJoinCode:
                case Action.RemoveMember:
                    return userRoles.Contains(_adminRole);

                case Action.AddMember:
                case Action.CreateMeeting:
                    acceptedRoles = new List<string> { _adminRole, _moderatorRole };
                    return userRoles.Intersect(acceptedRoles).Any();


                case Action.MemberAccess:

                    acceptedRoles = new List<string> { _adminRole, _moderatorRole, _memberRole, };
                    return userRoles.Intersect(acceptedRoles).Any();

            }

            return false;
        }
    }
}
