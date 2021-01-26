CREATE FUNCTION dbo.ufnGetTasksForMeeting(@MeetingId INT)
    RETURNS TABLE AS RETURN
        SELECT t.id AS Id, t.description AS Description, r.estimated_time AS EstimatedTime
        FROM task t
                 LEFT JOIN result r ON t.id = r.task_id
        WHERE t.meeting_id = @MeetingId
go


