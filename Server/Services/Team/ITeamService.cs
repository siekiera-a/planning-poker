﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.Models.Dapper;
using Server.Utils;

namespace Server.Services.Team
{
    public interface ITeamService
    {

        Task<List<TeamBase>> GetTeams();
        Task<bool> ChangeName(int teamId, string newName);
        Task<Optional<string>> GenerateJoinCode(int teamId);
        Task<bool> RemoveJoinCode(int teamId);
        Task<Optional<TeamBase>> CreateTeam(string name);
        Task<List<User>> GetMembers(int teamId);
    }
}
