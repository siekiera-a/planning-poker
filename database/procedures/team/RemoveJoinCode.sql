CREATE PROCEDURE dbo.spTeam_RemoveJoinCode @Id INT
AS
BEGIN
    UPDATE team SET join_code = NULL WHERE id = @Id
END
go


