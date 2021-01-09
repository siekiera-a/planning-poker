CREATE PROCEDURE dbo.spResult_AssignUserToTask @UserId INT,
                                               @TaskId INT,
                                               @EstimatedTime SMALLINT
AS
BEGIN
    SET NOCOUNT ON

    INSERT INTO result VALUES (@TaskId, @UserId, @EstimatedTime)
END
go


