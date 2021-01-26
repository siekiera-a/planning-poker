CREATE PROCEDURE dbo.spInvitation_InviteUser @MeetingId INT,
                                             @UserId INT
AS
BEGIN
    SET NOCOUNT ON

    IF EXISTS(SELECT *
              FROM meeting m
                       INNER JOIN team t ON m.team_id = t.id
              WHERE m.id = @MeetingId
                AND EXISTS(SELECT * FROM team_member WHERE team_id = t.id AND user_id = @UserId))
        BEGIN
                INSERT INTO invitation VALUES (@MeetingId, @UserId)
                SELECT 1
        END
END
go


