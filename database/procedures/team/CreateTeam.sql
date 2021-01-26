CREATE PROCEDURE dbo.spTeam_CreateTeam @Name NVARCHAR(100), @UserId INT
AS
BEGIN
    SET NOCOUNT ON

    DECLARE @Table table
                   (
                       id INT
                   )

    INSERT INTO team OUTPUT inserted.id INTO @Table VALUES (@Name, NULL)
    INSERT INTO team_member(team_id, user_id, role)
    VALUES ((SELECT id FROM @Table), @UserId, (SELECT id FROM role WHERE name = 'ADMIN'))
    SELECT id FROM @Table
END
go


