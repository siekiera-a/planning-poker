CREATE FUNCTION dbo.ufnIsTheMeetingOrganizer(@UserId INT, @MeetingId INT)
    RETURNS TABLE AS
        RETURN SELECT COUNT(*) AS IsOrganizer
               FROM meeting
               WHERE id = @MeetingId
                 AND organizer = @UserId
go


