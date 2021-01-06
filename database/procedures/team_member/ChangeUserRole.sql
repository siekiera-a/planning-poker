CREATE PROCEDURE dbo.spTeamMember_ChangeUserRole @TeamId INT,
                                                 @UserId INT,
                                                 @Role int
AS
BEGIN
    SET NOCOUNT ON

    UPDATE team_member SET role = @Role WHERE team_id = @TeamId AND user_id = @UserId
END
go


