create database planning_poker
go

use planning_poker
go

create table dbo.role
(
    id   int identity
        constraint role_pk
            primary key nonclustered,
    name varchar(20) not null
)
go

create unique index role_name_uindex
    on dbo.role (name)
go

create table dbo.team
(
    id        int identity
        constraint team_pk
            primary key nonclustered,
    name      nvarchar(100) not null,
    join_code varchar(8)
)
go

create table dbo.[user]
(
    id       int identity
        constraint user_pk
            primary key nonclustered,
    name     nvarchar(100) not null,
    email    nvarchar(100) not null,
    password binary(60)  not null
)
go

create table dbo.meeting
(
    id         int identity
        constraint meeting_pk
            primary key nonclustered,
    start_time datetime2(0) not null,
    end_time   datetime2(0),
    team_id    int          not null
        references dbo.team,
    organizer  int          not null
        references dbo.[user]
)
go

create table dbo.invitation
(
    meeting_id int not null
        references dbo.meeting,
    user_id    int not null
        references dbo.[user],
    primary key (meeting_id, user_id)
)
go

create table dbo.refresh_token
(
    token           varchar(50)
        constraint refresh_token_pk
            primary key nonclustered,
    expiration_time datetime2(0) not null,
    user_id         int          not null
        references dbo.[user]
)
go

create table dbo.task
(
    id          int identity
        constraint task_pk
            primary key nonclustered,
    description nvarchar(max) not null,
    meeting_id  int           not null
        references dbo.meeting
)
go

create table dbo.result
(
    task_id        int      not null
        primary key
        references dbo.task,
    user_id        int      not null
        references dbo.[user],
    estimated_time smallint not null
)
go

create table dbo.team_member
(
    team_id   int                                   not null
        references dbo.team,
    user_id   int                                   not null
        references dbo.[user],
    role      int          default 1                not null
        references dbo.role,
    join_time datetime2(0) default sysutcdatetime() not null,
    primary key (team_id, user_id)
)
go

create unique index user_email_uindex
    on dbo.[user] (email)
go


INSERT INTO role VALUES ('MEMBER'), ('ADMIN'), ('MODERATOR')
go
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



-- from: ./functions/GetMeetings.sql
-- przyszłe spotkania we wszystkich zespołach dla danego uzytkownika
CREATE FUNCTION dbo.ufnGetMeetings(@UserId INT)
    RETURNS TABLE AS
        RETURN
        SELECT m.id AS Id, m.start_time AS StartTime, m.organizer AS Organizer, m.team_id AS TeamId, t.name As TeamName
        FROM meeting m
                 JOIN invitation i on m.id = i.meeting_id
                 JOIN team t on m.team_id = t.id
        WHERE i.user_id = @UserId
          AND m.start_time > SYSUTCDATETIME()
go



-- from: ./functions/GetPastMeetingsForTeam.sql
-- wszystkie zakończone spotkania w zespole i ich wyniki
CREATE FUNCTION dbo.ufnGetPastMeetingsForTeam(@TeamId INT, @StartTime DATETIME2)
    RETURNS TABLE AS
        RETURN
        SELECT t.id             AS TeamId,
               t.name           AS Nam,
               m.start_time     AS StartTime,
               m.end_time       AS EndTime,
               u.name           AS 'User',
               u.email          AS Email,
               r.estimated_time AS EstimatedTime,
               ta.description   AS Description
        FROM team t
                 INNER JOIN meeting m ON t.id = m.team_id
                 INNER JOIN task ta on m.id = ta.meeting_id
                 INNER JOIN result r on ta.id = r.task_id
                 INNER JOIN [user] u on r.user_id = u.id
                 INNER JOIN [user] org on org.id = m.organizer
        WHERE t.id = @TeamId
          AND m.end_time BETWEEN @StartTime AND SYSUTCDATETIME()
go



-- from: ./functions/GetResultsForUser.sql
-- zwróci wszystkie wyniki powiązane z użytkownikiem z danego czasu
CREATE FUNCTION dbo.ufnGetResultsForUser(@UserId INT, @StartTime DATETIME2)
    RETURNS TABLE AS
        RETURN
        SELECT t.description    AS Description,
               r.estimated_time AS EstimatedTime,
               m.start_time     AS StartTime,
               m.end_time       AS EndTime,
               tm.name          AS TeamName,
               tm.id            AS TeamId
        FROM task t
                 INNER JOIN result r on t.id = r.task_id
                 INNER JOIN meeting m on t.meeting_id = m.id
                 INNER JOIN team tm ON m.team_id = tm.id
        WHERE r.user_id = @UserId
          AND m.end_time BETWEEN @StartTime AND SYSUTCDATETIME()
go



-- from: ./functions/GetRoles.sql
CREATE FUNCTION dbo.ufnGetRoles(@UserId INT, @TeamId INT)
    RETURNS TABLE AS
        RETURN SELECT r.name AS role
               FROM role r
                        INNER JOIN team_member tm on r.id = tm.role
               WHERE tm.user_id = @UserId
                 AND tm.team_id = @TeamId
go



-- from: ./functions/GetTeamMembers.sql
-- lista czlonkow dla zespolu
CREATE FUNCTION dbo.ufnGetTeamMembers(@TeamId INT)
    RETURNS TABLE AS
        RETURN
        SELECT u.id AS Id, u.name AS Name, u.email AS Email
        FROM team_member tm
                 join [user] u on tm.user_id = u.id
        WHERE tm.team_id = @TeamId
go



-- from: ./functions/GetTeams.sql
-- zespoły dla użytkownika
CREATE FUNCTION dbo.ufnGetTeams(@UserId INT)
    RETURNS TABLE AS
        RETURN
        SELECT t.id AS Id, t.name AS Name
        FROM team t
                 JOIN team_member tm on t.id = tm.team_id
                 JOIN [user] u on tm.user_id = u.id
        WHERE u.id = @UserId
go



-- from: ./functions/IsTheMeetingOrganizer.sql
CREATE FUNCTION dbo.ufnIsTheMeetingOrganizer(@UserId INT, @MeetingId INT)
    RETURNS TABLE AS
        RETURN SELECT COUNT(*) AS IsOrganizer
               FROM meeting
               WHERE id = @MeetingId
                 AND organizer = @UserId
go



-- from: ./procedures/invitation/InviteUser.sql
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
        SELECT id AS Id, start_time AS StartTime, end_time AS EndTime, team_id AS TeamId, organizer AS Organizer FROM meeting WHERE id = @Id

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
        SELECT @Id AS Id

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
CREATE PROCEDURE dbo.spTeam_CreateTeam @Name NVARCHAR(100), @UserId INT
AS
BEGIN
    SET NOCOUNT ON

    DECLARE @Table table
                   (
                       id INT
                   )

    INSERT INTO team OUTPUT inserted.id INTO @Table VALUES (@Name, NULL)
    INSERT INTO team_member(team_id, user_id, role)
    VALUES ((SELECT id FROM @Table), @UserId, (SELECT id FROM role WHERE name = 'ADMIN'))
    SELECT id FROM @Table
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
                                            @Email NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON

    DECLARE @Id INT
    SELECT @Id = id FROM [user] WHERE email = @Email

    IF @Id IS NOT NULL 
        -- create member with default role (1) and join_time (current utc time)
        INSERT INTO team_member (team_id, user_id) VALUES (@TeamId, @Id)
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



