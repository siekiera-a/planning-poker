CREATE PROCEDURE dbo.spTeamMember_JoinWithCode @UserId INT,
                                               @Code VARCHAR(8)
AS
BEGIN
    SET NOCOUNT ON

    DECLARE @TeamId INT

    -- check if exists team with given code
    SELECT @TeamId = id FROM team WHERE join_code = @Code

    IF @TeamId IS NOT NULL
        BEGIN
            INSERT INTO team_member(team_id, user_id) VALUES (@TeamId, @UserId)
            SELECT @TeamId
        END
END
go


