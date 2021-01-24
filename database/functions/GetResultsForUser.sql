-- zwróci wszystkie wyniki powiązane z użytkownikiem z danego czasu
CREATE FUNCTION dbo.ufnGetResultsForUser(@UserId INT, @StartTime DATETIME2)
    RETURNS TABLE AS
        RETURN
        SELECT t.description    AS Description,
               r.estimated_time AS EstimatedTime,
               m.start_time     AS StartTime,
               m.end_time       AS EndTime,
               tm.name          AS TeamName,
               tm.id            AS TeamId
        FROM task t
                 INNER JOIN result r on t.id = r.task_id
                 INNER JOIN meeting m on t.meeting_id = m.id
                 INNER JOIN team tm ON m.team_id = tm.id
        WHERE r.user_id = @UserId
          AND m.end_time IS NOT NULL 
          AND m.end_time BETWEEN @StartTime AND SYSUTCDATETIME()
go


