CREATE PROCEDURE dbo.spTask_EditTask @Id INT,
                                     @NewDescription NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON

    UPDATE task SET description = @NewDescription WHERE id = @Id
END
go


