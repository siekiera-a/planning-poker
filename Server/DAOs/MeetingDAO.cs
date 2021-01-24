﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using Server.Models.Dapper;
using Server.Services.DataAccess;
using Server.Utils;

namespace Server.DAOs
{
    public class MeetingDAO : BaseDAO
    {

        private readonly string _prefix = "dbo.spMeeting";
        private readonly ILogger<MeetingDAO> _logger;

        public MeetingDAO(IConnectionFactory connectionFactory, ILogger<MeetingDAO> logger) : base(connectionFactory)
        {
            _logger = logger;
        }

        public async Task<Optional<int>> CreateMeeting(DateTime startTime, int teamId, int userId)
        {
            using var connection = _connectionFactory.CreateConnection();

            try
            {
                var meetingId = await connection.QueryFirstAsync<int>($"{_prefix}_CreateMeeting",
                    new { StartTime = startTime, TeamId = teamId, Organizer = userId },
                    commandType: CommandType.StoredProcedure);
                return Optional<int>.of(meetingId);
            }
            catch (SqlException e)
            {
                _logger.LogError(e.Message);
                return Optional<int>.Empty();
            }
        }

        public async Task<Optional<Meeting>> EndMeeting(int meetingId)
        {
            using var connection = _connectionFactory.CreateConnection();

            try
            {
                var meeting = await connection.QueryFirstOrDefaultAsync<Meeting>($"{_prefix}_EndMeeting",
                    new { Id = meetingId },
                    commandType: CommandType.StoredProcedure);
                return Optional<Meeting>.ofNullable(meeting);
            }
            catch (SqlException e)
            {
                _logger.LogError(e.Message);
                return Optional<Meeting>.Empty();
            }
        }

        public async Task<bool> RemoveMeeting(int meetingId)
        {
            using var connection = _connectionFactory.CreateConnection();

            try
            {
                await connection.ExecuteAsync($"{_prefix}_RemoveMeeting", new { Id = meetingId },
                    commandType: CommandType.StoredProcedure);
                return true;
            }
            catch (SqlException e)
            {
                _logger.LogError(e.Message);
                return false;
            }
        }

        public async Task<bool> RescheduleMeeting(int meetingId, DateTime newStartTime)
        {
            using var connection = _connectionFactory.CreateConnection();

            try
            {
                var id = await connection.QueryFirstOrDefaultAsync<int>($"{_prefix}_RescheduleMeeting", new { Id = meetingId, NewStartTime = newStartTime },
                    commandType: CommandType.StoredProcedure);
                return id != 0;
            }
            catch (SqlException e)
            {
                _logger.LogError(e.Message);
                return false;
            }
        }

    }
}
