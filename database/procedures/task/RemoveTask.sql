CREATE PROCEDURE dbo.spTask_RemoveTask @Id INT
AS
BEGIN
    SET NOCOUNT ON

    DELETE FROM task WHERE id = @Id
END
go


