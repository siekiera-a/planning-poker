CREATE FUNCTION dbo.ufnJoinMeetingPermission(@UserId INT, @MeetingId INT)
    RETURNS TABLE AS RETURN
        SELECT u.name                          AS Name,
               u.email                         AS Email,
               (IIF(u.id = m.organizer, 1, 0)) AS IsOrganizer,
               1                               AS IsInvited
        FROM invitation i
                 JOIN [user] u ON i.user_id = u.id
                 JOIN meeting m on i.meeting_id = m.id
        WHERE i.user_id = @UserId
          AND i.meeting_id = @MeetingId
go


