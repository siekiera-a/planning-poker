-- wszystkie zakończone spotkania w zespole i ich wyniki
CREATE FUNCTION dbo.ufnGetPastMeetingsForTeam(@TeamId INT, @StartTime DATETIME2)
    RETURNS TABLE AS
        RETURN
        SELECT t.id             AS TeamId,
               t.name           AS Nam,
               m.start_time     AS StartTime,
               m.end_time       AS EndTime,
               u.name           AS 'User',
               u.email          AS Email,
               r.estimated_time AS EstimatedTime,
               ta.description   AS Description
        FROM team t
                 INNER JOIN meeting m ON t.id = m.team_id
                 INNER JOIN task ta on m.id = ta.meeting_id
                 INNER JOIN result r on ta.id = r.task_id
                 INNER JOIN [user] u on r.user_id = u.id
                 INNER JOIN [user] org on org.id = m.organizer
        WHERE t.id = @TeamId
          AND m.end_time BETWEEN @StartTime AND SYSUTCDATETIME()
go


