CREATE PROCEDURE dbo.spTeam_ChangeName @Id INT,
                                       @NewName NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON

    UPDATE team SET name = @NewName WHERE id = @Id
END
go


