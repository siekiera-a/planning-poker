CREATE PROCEDURE dbo.spTeamMember_RemoveMember @TeamId INT,
                                               @UserId INT
AS
BEGIN
    SET NOCOUNT ON

    DELETE FROM team_member WHERE team_id = @TeamId AND user_id = @UserId
    -- delete invitations for future meetings
    DELETE
    FROM invitation
    WHERE id IN
          (SELECT i.id
           FROM invitation i
                    INNER JOIN meeting m ON i.meeting_id = m.id
           WHERE m.start_time >= SYSUTCDATETIME()
             AND i.user_id = @UserId
             AND m.team_id = @TeamId)
END
go


