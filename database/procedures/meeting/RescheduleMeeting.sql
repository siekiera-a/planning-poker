CREATE PROCEDURE dbo.spMeeting_RescheduleMeeting @Id INT,
                                                 @NewStartTime DATETIME2(0)
AS
BEGIN
    SET NOCOUNT ON

    -- meeting cannot be reschedule to the past time
    IF @NewStartTime > SYSUTCDATETIME()
        BEGIN
            UPDATE meeting SET start_time = @NewStartTime WHERE id = @Id
            SELECT @Id AS Id
        END

END
go


