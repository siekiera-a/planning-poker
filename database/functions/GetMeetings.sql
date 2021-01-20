-- przyszłe spotkania we wszystkich zespołach dla danego uzytkownika
CREATE FUNCTION dbo.ufnGetMeetings(@UserId INT)
    RETURNS TABLE AS
        RETURN
        SELECT m.id AS Id, m.start_time AS StartTime, m.organizer AS Organizer, m.team_id AS TeamId, t.name As TeamName
        FROM meeting m
                 JOIN invitation i on m.id = i.meeting_id
                 JOIN team t on m.team_id = t.id
        WHERE i.user_id = @UserId
          AND m.start_time > SYSUTCDATETIME()
go


