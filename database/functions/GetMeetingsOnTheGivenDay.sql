CREATE FUNCTION dbo.ufnGetMeetingsOnTheGivenDay(@UserId INT, @Date DATE)
    RETURNS TABLE AS
        RETURN
        SELECT m.id         AS Id,
               m.start_time AS StartTime,
               m.end_time   AS EndTime,
               m.organizer  AS OrganizerId,
               u.name       AS OrganizerName,
               m.team_id    AS TeamId,
               t.name       As TeamName
        FROM meeting m
                 JOIN invitation i on m.id = i.meeting_id
                 JOIN team t on m.team_id = t.id
                 JOIN [user] u on u.id = m.organizer
        WHERE i.user_id = @UserId
          AND CONVERT(DATE, m.start_time) = @Date
go


