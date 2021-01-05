create database planning_poker
go

use planning_poker
go

create table dbo.invitation
(
    id         int identity
        constraint invitation_pk
            primary key nonclustered,
    meeting_id int not null,
    user_id    int not null
)
go

create table dbo.meeting
(
    id         int identity
        constraint meeting_pk
            primary key nonclustered,
    start_time datetime2(0) not null,
    end_time   datetime2(0),
    team_id    int          not null,
    organizer  int          not null
)
go

create table dbo.refresh_token
(
    id              int identity
        constraint refresh_token_pk
            primary key nonclustered,
    token           varchar(50)  not null,
    expiration_time datetime2(0) not null,
    user_id         int          not null
)
go

create table dbo.result
(
    id             int identity
        constraint result_pk
            primary key nonclustered,
    task_id        int      not null,
    user_id        int      not null,
    estimated_time smallint not null
)
go

create table dbo.role
(
    id      int identity
        constraint role_pk
            primary key nonclustered,
    name    varchar(20) not null          
)
go

create table dbo.task
(
    id          int identity
        constraint task_pk
            primary key nonclustered,
    description nvarchar(max) not null,
    meeting_id  int           not null
)
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

create unique index team_join_code_uindex
    on dbo.team (join_code)
go

create table dbo.team_member
(
    id      int identity
        constraint team_member_pk
            primary key nonclustered,
    team_id int not null,
    user_id int not null,
    role    int not null
)
go

create table dbo.[user]
(
    id       int identity
        constraint user_pk
            primary key nonclustered,
    name     nvarchar(100) not null,
    email    nvarchar(100) not null,
    password varchar(100)  not null
)
go

create unique index user_email_uindex
    on dbo.[user] (email)
go


