CREATE PROCEDURE dbo.spMeeting_CreateMeeting @StartTime DATETIME2(0),
                                             @TeamId INT,
                                             @Organizer INT
AS
BEGIN
    SET NOCOUNT ON

    INSERT INTO meeting VALUES (@StartTime, NULL, @TeamId, @Organizer)
END
go


