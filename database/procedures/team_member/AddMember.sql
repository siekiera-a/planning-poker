CREATE PROCEDURE dbo.spTeamMember_AddMember @TeamId INT,
                                            @UserId INT
AS
BEGIN
    SET NOCOUNT ON

    -- create member with default role (1) and join_time (current utc time)
    INSERT INTO team_member (team_id, user_id) VALUES (@TeamId, @UserId)
END
go


