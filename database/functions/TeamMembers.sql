-- lista czlonkow dla zespolu
CREATE FUNCTION dbo.ufnTeamMembers(@TeamId INT)
    RETURNS TABLE AS
        RETURN
        SELECT u.id AS Id, u.name AS Name, u.email AS Email
        FROM team_member tm
                 join [user] u on tm.user_id = u.id
        WHERE tm.team_id = @TeamId
go


