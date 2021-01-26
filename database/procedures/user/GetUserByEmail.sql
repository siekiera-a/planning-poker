CREATE PROCEDURE dbo.spUser_GetUserByEmail @Email NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON

    SELECT id, name, email, password FROM [user] WHERE email = @Email
END
go


