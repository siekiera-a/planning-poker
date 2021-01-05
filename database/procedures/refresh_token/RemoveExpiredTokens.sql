CREATE PROCEDURE dbo.spRefreshToken_RemoveExpiredTokens
AS
BEGIN
    DELETE FROM refresh_token WHERE expiration_time <= SYSDATETIME()
END
go


