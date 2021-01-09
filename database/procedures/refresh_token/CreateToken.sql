CREATE PROCEDURE dbo.spRefreshToken_CreateToken @UserId INT,
                                                       @Token VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON

    -- refresh token is valid only for day
    INSERT INTO refresh_token VALUES (@Token, DATEADD(dd, 1, SYSUTCDATETIME()), @UserId)
END
go


