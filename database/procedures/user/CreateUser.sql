CREATE PROCEDURE dbo.spUser_CreateUser @Name NVARCHAR(100),
                                       @Email NVARCHAR(100),
                                       @Password VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON

    INSERT INTO [user] OUTPUT inserted.id VALUES (@Name, @Email, @Password)

END
go


