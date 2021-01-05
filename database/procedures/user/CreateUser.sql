CREATE PROCEDURE dbo.spUser_CreateUser @Name NVARCHAR(100),
                                       @Email NVARCHAR(100),
                                       @Password VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON
    DECLARE @Success BIT = 1

    IF (SELECT COUNT(*) FROM [user] WHERE email = @Email) > 0
        SET @Success = 0
    ELSE
        INSERT INTO [user] VALUES (@Name, @Email, @Password)

    SELECT @Success
END
go


