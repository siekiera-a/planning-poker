CREATE PROCEDURE dbo.spInvitation_InviteUser @MeetingId INT,
                                             @UserId INT
AS
BEGIN
    SET NOCOUNT ON

    INSERT INTO invitation VALUES (@MeetingId, @UserId)
END
go


