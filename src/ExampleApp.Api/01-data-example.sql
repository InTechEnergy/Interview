INSERT INTO Semesters (Id, Description, StartDate, EndDate)
VALUES 
	(1, 'Fall 2023', '2023-03-01', '2023-09-01'), 
	(2, 'Spring 2023', '2023-09-01', '2023-03-01');

INSERT INTO Professors (FullName, Extension)
VALUES ('John Smith', 'prof1'), ('Jane Smith', 'prof2');

INSERT INTO Courses (Id, Description, SemesterId, ProfessorId, CreatedOn, LastModifiedOn)
VALUES ('course1', 'Math', 1, 1, GETDATE(), GETDATE()), ('course2', 'English', 2, 2, GETDATE(), GETDATE());

INSERT INTO Students (FullName, Badge, ResidentStatus)
VALUES ('John Doe', '123', 1), ('Jane Doe', '456', 2);

DECLARE @StudentId INT;
SELECT @StudentId = Id FROM Students WHERE FullName = 'John Doe';

INSERT INTO StudentCourses (StudentId, CourseId)
VALUES (@StudentId, 'course1'), (@StudentId, 'course2');
