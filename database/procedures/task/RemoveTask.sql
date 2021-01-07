CREATE PROCEDURE dbo.spTask_RemoveTask @Id INT
AS
BEGIN
    SET NOCOUNT ON
    
    -- task can be removed only if it is not assigned to user
    IF (NOT EXISTS (SELECT * FROM result WHERE task_id = @Id))
        DELETE FROM task WHERE id = @Id
END
go


