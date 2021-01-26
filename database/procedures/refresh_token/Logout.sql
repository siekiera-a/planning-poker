CREATE PROCEDURE dbo.spRefreshToken_Logout @UserId INT
AS
BEGIN
    SET NOCOUNT ON

    DELETE FROM refresh_token WHERE user_id = @UserId
END
go


