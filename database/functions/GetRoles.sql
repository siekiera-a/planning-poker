CREATE FUNCTION dbo.ufnGetRoles(@UserId INT, @TeamId INT)
    RETURNS TABLE AS
        RETURN SELECT r.name AS role
               FROM role r
                        INNER JOIN team_member tm on r.id = tm.role
               WHERE tm.user_id = @UserId
                 AND tm.team_id = @TeamId
go


