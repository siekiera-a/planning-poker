CREATE PROCEDURE dbo.spTeam_CreateTeam @Name NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON

    INSERT INTO team OUTPUT inserted.id VALUES (@Name, NULL)
END
go


