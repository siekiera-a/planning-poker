CREATE PROCEDURE dbo.spInvitation_RemoveInvitation @MeetingId INT,
                                                   @UserId INT
AS
BEGIN
    SET NOCOUNT ON

    DELETE FROM invitation WHERE user_id = @UserId AND meeting_id = @MeetingId
END
go


