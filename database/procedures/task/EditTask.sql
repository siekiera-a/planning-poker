CREATE PROCEDURE dbo.spTask_EditTask @Id INT,
                                     @NewDescription NVARCHAR(MAX)
AS
BEGIN
    UPDATE task SET description = @NewDescription WHERE id = @Id
END
go


