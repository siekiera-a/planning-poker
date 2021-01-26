CREATE PROCEDURE dbo.spUser_CreateUser @Name NVARCHAR(100),
                                       @Email NVARCHAR(100),
                                       @Password BINARY(60)
AS
BEGIN
    SET NOCOUNT ON

    DECLARE @Table table
                   (
                       id INT
                   )

    INSERT INTO [user] OUTPUT inserted.id INTO @Table VALUES (@Name, @Email, @Password)

    SELECT (SELECT id from @Table) as id, @Name as name, @Email as email
END
go


