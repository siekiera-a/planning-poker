-- from: ./views/NewId.sql
CREATE VIEW dbo.NewId as
SELECT NEWID() as new_id
go



-- from: ./functions/GenerateRandomString.sql
CREATE FUNCTION dbo.ufnGenerateRandomString(@Length INT) RETURNS VARCHAR(max)
AS
BEGIN
    DECLARE
        @String VARCHAR(max),
        @i INT = 0,
        @AvailableChars VARCHAR(max) = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789'

    DECLARE
        @CharsCount INT = LEN(@AvailableChars)

    WHILE @i < @Length
        BEGIN
            SET @String = CONCAT(@String,
                                 SUBSTRING(@AvailableChars, ABS(CHECKSUM((SELECT new_id FROM NewId))) % @CharsCount + 1,
                                           1))
            SET @i = @i + 1
        END

    RETURN @String
END
go



-- from: ./procedures/invitation/InviteUser.sql
CREATE PROCEDURE dbo.spInvitation_InviteUser @MeetingId INT,
                                             @UserId INT
AS
BEGIN
    SET NOCOUNT ON

    INSERT INTO invitation VALUES (@MeetingId, @UserId)
END
go



-- from: ./procedures/invitation/RemoveInvitation.sql
CREATE PROCEDURE dbo.spInvitation_RemoveInvitation @MeetingId INT,
                                                   @UserId INT
AS
BEGIN
    SET NOCOUNT ON

    DELETE FROM invitation WHERE user_id = @UserId AND meeting_id = @MeetingId
END
go



-- from: ./procedures/meeting/CreateMeeting.sql
CREATE PROCEDURE dbo.spMeeting_CreateMeeting @StartTime DATETIME2(0),
                                             @TeamId INT,
                                             @Organizer INT
AS
BEGIN
    SET NOCOUNT ON

    INSERT INTO meeting OUTPUT inserted.id VALUES (@StartTime, NULL, @TeamId, @Organizer)
END
go



-- from: ./procedures/meeting/EndMeeting.sql
CREATE PROCEDURE dbo.spMeeting_EndMeeting @Id INT
AS
BEGIN
    SET NOCOUNT ON

    DECLARE
        @Time DATETIME2(0) = SYSUTCDATETIME(),
        @Start DATETIME2(0),
        @End DATETIME2(0)

    SELECT @Start = start_time, @End = end_time FROM meeting WHERE id = @Id

    IF @Start < @Time AND @End IS NULL
        UPDATE meeting SET end_time = @Time WHERE id = @Id

    SELECT id, start_time, end_time, team_id, organizer FROM meeting WHERE id = @Id
END
go



-- from: ./procedures/meeting/RemoveMeeting.sql
CREATE PROCEDURE dbo.spMeeting_RemoveMeeting @Id INT
AS
BEGIN
    SET NOCOUNT ON

    -- remove meeting only if not started
    IF ((SELECT start_time FROM meeting WHERE id = @Id) > SYSUTCDATETIME())
        BEGIN
            DELETE FROM invitation WHERE meeting_id = @Id
            DELETE FROM task WHERE meeting_id = @Id
            DELETE FROM meeting WHERE id = @Id
        END
END
go



-- from: ./procedures/meeting/RescheduleMeeting.sql
CREATE PROCEDURE dbo.spMeeting_RescheduleMeeting @Id INT,
                                                 @NewStartTime DATETIME2(0)
AS
BEGIN
    SET NOCOUNT ON

    -- meeting cannot be reschedule to the past time
    IF @NewStartTime > SYSUTCDATETIME()
        UPDATE meeting SET start_time = @NewStartTime WHERE id = @Id

END
go



-- from: ./procedures/refresh_token/CreateToken.sql
CREATE PROCEDURE dbo.spRefreshToken_CreateToken @UserId INT,
                                                       @Token VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON

    -- refresh token is valid only for day
    INSERT INTO refresh_token VALUES (@Token, DATEADD(dd, 1, SYSUTCDATETIME()), @UserId)
END
go



-- from: ./procedures/refresh_token/Logout.sql
CREATE PROCEDURE dbo.spRefreshToken_Logout @UserId INT
AS
BEGIN
    SET NOCOUNT ON

    DELETE FROM refresh_token WHERE user_id = @UserId
END
go



-- from: ./procedures/refresh_token/RemoveExpiredTokens.sql
CREATE PROCEDURE dbo.spRefreshToken_RemoveExpiredTokens
AS
BEGIN
    SET NOCOUNT ON

    DELETE FROM refresh_token WHERE expiration_time <= SYSUTCDATETIME()
END
go



-- from: ./procedures/result/AssignUserToTask.sql
CREATE PROCEDURE dbo.spResult_AssignUserToTask @UserId INT,
                                               @TaskId INT,
                                               @EstimatedTime SMALLINT
AS
BEGIN
    SET NOCOUNT ON

    INSERT INTO result VALUES (@TaskId, @UserId, @EstimatedTime)
END
go



-- from: ./procedures/task/AddTask.sql
CREATE PROCEDURE dbo.spTask_AddTask @Description NVARCHAR(MAX),
                                    @MeetingId INT
AS
BEGIN
    SET NOCOUNT ON

    INSERT INTO task VALUES (@Description, @MeetingId)
END
go



-- from: ./procedures/task/EditTask.sql
CREATE PROCEDURE dbo.spTask_EditTask @Id INT,
                                     @NewDescription NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON

    UPDATE task SET description = @NewDescription WHERE id = @Id
END
go



-- from: ./procedures/task/RemoveTask.sql
CREATE PROCEDURE dbo.spTask_RemoveTask @Id INT
AS
BEGIN
    SET NOCOUNT ON
    
    -- task can be removed only if it is not assigned to user
    IF (NOT EXISTS (SELECT * FROM result WHERE task_id = @Id))
        DELETE FROM task WHERE id = @Id
END
go



-- from: ./procedures/team/ChangeName.sql
CREATE PROCEDURE dbo.spTeam_ChangeName @Id INT,
                                       @NewName NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON

    UPDATE team SET name = @NewName WHERE id = @Id
END
go



-- from: ./procedures/team/CreateTeam.sql
CREATE PROCEDURE dbo.spTeam_CreateTeam @Name NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON

    INSERT INTO team OUTPUT inserted.id VALUES (@Name, NULL)
END
go



-- from: ./procedures/team/GenerateJoinCode.sql
CREATE PROCEDURE dbo.spTeam_GenerateJoinCode @Id INT
AS
BEGIN
    SET NOCOUNT ON

    DECLARE @NewCode VARCHAR(8) = dbo.ufnGenerateRandomString(8)

    WHILE @NewCode IN (SELECT join_code FROM team)
        SET @NewCode = dbo.ufnGenerateRandomString(8)

    UPDATE team SET join_code = @NewCode WHERE id = @Id
    SELECT @NewCode

END
go



-- from: ./procedures/team/RemoveJoinCode.sql
CREATE PROCEDURE dbo.spTeam_RemoveJoinCode @Id INT
AS
BEGIN
    SET NOCOUNT ON

    UPDATE team SET join_code = NULL WHERE id = @Id
END
go



-- from: ./procedures/team_member/AddMember.sql
CREATE PROCEDURE dbo.spTeamMember_AddMember @TeamId INT,
                                            @UserId INT
AS
BEGIN
    SET NOCOUNT ON

    -- create member with default role (1) and join_time (current utc time)
    INSERT INTO team_member (team_id, user_id) VALUES (@TeamId, @UserId)
END
go



-- from: ./procedures/team_member/ChangeUserRole.sql
CREATE PROCEDURE dbo.spTeamMember_ChangeUserRole @TeamId INT,
                                                 @UserId INT,
                                                 @Role int
AS
BEGIN
    SET NOCOUNT ON

    UPDATE team_member SET role = @Role WHERE team_id = @TeamId AND user_id = @UserId
END
go



-- from: ./procedures/team_member/JoinWithCode.sql
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
        EXEC dbo.spTeamMember_AddMember @TeamId, @UserId
        SELECT @TeamId
    END
END
go



-- from: ./procedures/team_member/RemoveMember.sql
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



-- from: ./procedures/user/CreateUser.sql
CREATE PROCEDURE dbo.spUser_CreateUser @Name NVARCHAR(100),
                                       @Email NVARCHAR(100),
                                       @Password BINARY(60)
AS
BEGIN
    SET NOCOUNT ON

    DECLARE @Table table
                   (
                       id INT
                   )

    INSERT INTO [user] OUTPUT inserted.id INTO @Table VALUES (@Name, @Email, @Password)

    SELECT (SELECT id from @Table) as id, @Name as name, @Email as email
END
go



-- from: ./procedures/user/GetUserByEmail.sql
CREATE PROCEDURE dbo.spUser_GetUserByEmail @Email NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON

    SELECT id, name, email, password FROM [user] WHERE email = @Email
END
go


