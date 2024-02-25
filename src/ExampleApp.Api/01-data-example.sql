INSERT INTO Semesters (Id, Description, StartDate, EndDate)
VALUES 
	(1, 'Spring 2023', '2023-01-10', '2023-04-25'), 
	(2, 'Fall 2023', '2023-08-25', '2023-12-10'), 
	(3, 'Spring 2024', '2024-01-15', '2024-05-05'), 
	(4, 'Fall 2024', '2024-09-01', '2024-12-15');

INSERT INTO Professors (FullName, Extension)
VALUES
	('John Smith', 'prof1'),
	('Mary Lu', 'prof2');

INSERT INTO Courses (Id, Description, SemesterId, ProfessorId, CreatedOn, LastModifiedOn)
VALUES
	('course1', 'Math', 1, 1, GETDATE(), GETDATE()),
	('course2', 'English', 2, 2, GETDATE(), GETDATE()),
	('course3', 'Spanish', 3, 1, GETDATE(), GETDATE()),
	('course4', 'French', 3, 2, GETDATE(), GETDATE());

INSERT INTO Students (FullName, Badge, ResidentStatus)
VALUES
	('Sevann Radhak', 'A123', 1),
	('Dylan Rametta', 'A456', 2),
	('Melman Arigato', 'B001', 3),
	('Marty Pelagato', 'B002', 3);

DECLARE @StudentId1 INT;
DECLARE @StudentId2 INT;
SELECT @StudentId1 = Id FROM Students WHERE FullName = 'Sevann Radhak';
SELECT @StudentId2 = Id FROM Students WHERE FullName = 'Melman Arigato';


INSERT INTO StudentCourses (StudentId, CourseId)
VALUES
	(@StudentId1, 'course3'),
	(@StudentId1, 'course4'),
	(@StudentId2, 'course3');
