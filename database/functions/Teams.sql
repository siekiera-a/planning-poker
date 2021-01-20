-- zespoły dla użytkownika
CREATE FUNCTION dbo.ufnTeams(@UserId INT)
    RETURNS TABLE AS
        RETURN
        SELECT t.id AS Id, t.name AS Name
        FROM team t
                 JOIN team_member tm on t.id = tm.team_id
                 JOIN [user] u on tm.user_id = u.id
        WHERE u.id = @UserId
go


