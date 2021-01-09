CREATE PROCEDURE dbo.spRefreshToken_RemoveExpiredTokens
AS
BEGIN
    SET NOCOUNT ON

    DELETE FROM refresh_token WHERE expiration_time <= SYSUTCDATETIME()
END
go


