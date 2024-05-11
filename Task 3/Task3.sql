SELECT *
FROM StudyGroups
WHERE EXISTS (
    SELECT 1
    FROM Users
    WHERE Users.StudyGroupId = StudyGroups.id
    AND Users.name LIKE 'M%'
)
ORDER BY creation_date;
