CREATE PROCEDURE dbo.spTeam_GenerateJoinCode @Id INT
AS
BEGIN
    SET NOCOUNT ON

    DECLARE @NewCode VARCHAR(8) = dbo.ufnGenerateRandomString(8)

    WHILE @NewCode IN (SELECT join_code FROM team)
        SET @NewCode = dbo.ufnGenerateRandomString(8)

    UPDATE team SET join_code = @NewCode WHERE id = @Id
    SELECT @NewCode

END
go


