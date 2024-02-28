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
	('course4', 'French', 3, 2, GETDATE(), GETDATE()),
	('course5', 'Physics', 1, 2, GETDATE(), GETDATE()),
	('course6', 'Chemistry', 2, 3, GETDATE(), GETDATE()),
    ('course7', 'Biology', 2, 4, GETDATE(), GETDATE());



INSERT INTO Students (FullName, Badge, ResidentStatus)
VALUES
	('Sevann Radhak', 'A123', 1),
	('Dylan Rametta', 'A456', 2),
	('Melman Arigato', 'B001', 3),
	('Marty Pelagato', 'B002', 3),
	('John Doe', 'Badge1001', 1),
    ('Jane Smith', 'Badge1002', 2),
    ('Robert Johnson', 'Badge1003', 3),
    ('Michael Williams', 'Badge1004', 1),
    ('Sarah Brown', 'Badge1005', 2),
    ('Emily Davis', 'Badge1006', 3),
    ('Daniel Miller', 'Badge1007', 1),
    ('Emma Wilson', 'Badge1008', 2),
    ('David Moore', 'Badge1009', 3),
    ('Sophia Taylor', 'Badge1010', 1);

DECLARE @StudentId1 INT;
DECLARE @StudentId2 INT;
SELECT @StudentId1 = Id FROM Students WHERE FullName = 'Sevann Radhak';
SELECT @StudentId2 = Id FROM Students WHERE FullName = 'Melman Arigato';


INSERT INTO StudentCourses (StudentId, CourseId)
VALUES
	(@StudentId1, 'course3'),
	(@StudentId1, 'course4'),
	(@StudentId2, 'course3');
