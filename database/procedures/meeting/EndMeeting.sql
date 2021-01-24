CREATE PROCEDURE dbo.spMeeting_EndMeeting @Id INT
AS
BEGIN
    SET NOCOUNT ON

    DECLARE
        @Time DATETIME2(0) = SYSUTCDATETIME(),
        @Start DATETIME2(0),
        @End DATETIME2(0)

    SELECT @Start = start_time, @End = end_time FROM meeting WHERE id = @Id

    IF @Start < @Time AND @End IS NULL
        UPDATE meeting SET end_time = @Time WHERE id = @Id
        SELECT id AS Id, start_time AS StartTime, end_time AS EndTime, team_id AS TeamId, organizer AS Organizer FROM meeting WHERE id = @Id

END
go


