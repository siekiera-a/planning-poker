CREATE PROCEDURE dbo.spTeamMember_AddMember @TeamId INT,
                                            @UserId INT
AS
BEGIN
    SET NOCOUNT ON

    -- check if user is already in team
    IF (SELECT COUNT(*) FROM team_member WHERE team_id = @TeamId AND user_id = @UserId) = 0
        -- create member with default role (1)
        INSERT INTO team_member (team_id, user_id) VALUES (@TeamId, @UserId)
END
go


