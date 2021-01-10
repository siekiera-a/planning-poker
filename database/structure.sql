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


