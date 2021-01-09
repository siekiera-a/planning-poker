CREATE PROCEDURE dbo.spMeeting_RemoveMeeting @Id INT
AS
BEGIN
    SET NOCOUNT ON

    -- remove meeting only if not started
    IF ((SELECT start_time FROM meeting WHERE id = @Id) > SYSUTCDATETIME())
        BEGIN
            DELETE FROM invitation WHERE meeting_id = @Id
            DELETE FROM task WHERE meeting_id = @Id
            DELETE FROM meeting WHERE id = @Id
        END
END
go


