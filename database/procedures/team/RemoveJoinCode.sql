CREATE PROCEDURE dbo.spTeam_RemoveJoinCode @Id INT
AS
BEGIN
    SET NOCOUNT ON

    UPDATE team SET join_code = NULL WHERE id = @Id
END
go


