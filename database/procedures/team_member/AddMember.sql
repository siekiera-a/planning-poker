CREATE PROCEDURE dbo.spTeamMember_AddMember @TeamId INT,
                                            @Email NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON

    DECLARE @Id INT
    SELECT @Id = id FROM [user] WHERE email = @Email

    IF @Id IS NOT NULL
        BEGIN
            -- create member with default role (1) and join_time (current utc time)
            INSERT INTO team_member (team_id, user_id) VALUES (@TeamId, @Id)
            SELECT 1
        END
END
go


