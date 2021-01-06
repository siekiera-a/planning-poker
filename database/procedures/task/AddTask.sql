CREATE PROCEDURE dbo.spTask_AddTask @Description NVARCHAR(MAX),
                                    @MeetingId INT
AS
BEGIN
    SET NOCOUNT ON

    INSERT INTO task VALUES (@Description, @MeetingId)
END
go


