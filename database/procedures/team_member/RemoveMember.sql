CREATE PROCEDURE dbo.spTeamMember_RemoveMember @TeamId INT,
                                               @UserId INT
AS
BEGIN
    SET NOCOUNT ON

    DELETE FROM team_member WHERE team_id = @TeamId AND user_id = @UserId
    -- delete invitations for future meetings
    DELETE
    FROM invitation
    WHERE user_id = @UserId
      AND meeting_id IN
          (SELECT id FROM meeting WHERE start_time >= SYSUTCDATETIME() AND team_id = @TeamId)
END
go


