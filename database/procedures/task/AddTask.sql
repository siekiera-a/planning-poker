CREATE PROCEDURE dbo.spTask_AddTask @Description NVARCHAR(MAX),
                                    @MeetingId INT
AS
BEGIN
    SET NOCOUNT ON

    INSERT INTO task OUTPUT inserted.id VALUES (@Description, @MeetingId)
END
go


