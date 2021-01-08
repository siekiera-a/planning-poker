CREATE FUNCTION dbo.ufnGenerateRandomString(@Length INT) RETURNS VARCHAR(max)
AS
BEGIN
    DECLARE
        @String VARCHAR(max),
        @i INT = 0,
        @AvailableChars VARCHAR(max) = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789'

    DECLARE
        @CharsCount INT = LEN(@AvailableChars)

    WHILE @i < @Length
        BEGIN
            SET @String = CONCAT(@String,
                                 SUBSTRING(@AvailableChars, ABS(CHECKSUM((SELECT new_id FROM NewId))) % @CharsCount + 1,
                                           1))
            SET @i = @i + 1
        END

    RETURN @String
END
go


