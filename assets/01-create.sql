IF db_id('example-db') IS NULL
    CREATE DATABASE [example-db]

GO

/*
 ALTER TABLE [example-db].dbo.Courses DROP CONSTRAINT Courses_Professors;
 ALTER TABLE [example-db].dbo.Courses DROP CONSTRAINT Courses_Semesters;
 DROP TABLE [example-db].dbo.Courses;
 DROP TABLE [example-db].dbo.Students;
 DROP TABLE [example-db].dbo.Semester;
 DROP TABLE [example-db].dbo.Professors;

*/

CREATE TABLE [example-db].dbo.Students (
    Id INTEGER PRIMARY KEY IDENTITY,
    FullName NVARCHAR(100) NOT NULL,
    Badge VARCHAR(50) NOT NULL,
    ResidenceStatus VARCHAR(20) NOT NULL,
    CreatedOn DATETIMEOFFSET(7) NOT NULL DEFAULT SYSDATETIMEOFFSET(),
    LastModifiedOn DATETIMEOFFSET(7) NOT NULL DEFAULT SYSDATETIMEOFFSET()
    );

INSERT INTO [example-db].dbo.Students
  (FullName, Badge, ResidenceStatus, CreatedOn, LastModifiedOn)
  VALUES
    ('Player One', 'player-one', 'InState', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET()),
    ('Santa I Claus', 'santa-i-claus', 'Foreign', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET()),
    ('Alf', 'alf', 'OutOfState', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET())
;

CREATE TABLE [example-db].dbo.Semesters (
    Id VARCHAR(6) PRIMARY KEY,
    [Description] VARCHAR(20) NULL,
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    CreatedOn DATETIMEOFFSET(7) NOT NULL DEFAULT SYSDATETIMEOFFSET(),
    LastModifiedOn DATETIMEOFFSET(7) NOT NULL DEFAULT SYSDATETIMEOFFSET()
);

INSERT INTO [example-db].dbo.Semesters
  (Id, Description, StartDate, EndDate)
  VALUES
    ('2023-1', 'Spring ''23', '2023-01-09', '2023-06-30'),
    ('2023-2', 'Fall ''23', '2023-08-14', '2023-12-15'),
    ('2024-1', 'Spring ''24', '2024-01-08', '2024-06-28')
;

CREATE TABLE [example-db].dbo.Professors (
    Id INTEGER PRIMARY KEY IDENTITY,
    FullName NVARCHAR(100) NOT NULL,
    Extension NVARCHAR(6) NULL,
    CreatedOn DATETIMEOFFSET(7) NOT NULL DEFAULT SYSDATETIMEOFFSET(),
    LastModifiedOn DATETIMEOFFSET(7) NOT NULL DEFAULT SYSDATETIMEOFFSET()
);

SET IDENTITY_INSERT [example-db].dbo.Professors ON;
INSERT INTO [example-db].dbo.Professors
  (Id, FullName, Extension)
  VALUES
    (1, 'Severus Snape', '4851'),
    (2, 'Charles Xavier', NULL),
    (3, 'Horace Slughorn', '4850')
;
SET IDENTITY_INSERT [example-db].dbo.Professors OFF;

CREATE TABLE [example-db].dbo.Courses (
    Id VARCHAR(20) PRIMARY KEY,
    [Description] VARCHAR(100) NULL,
    ProfessorId INTEGER NOT NULL,
    SemesterId VARCHAR(6) NOT NULL,
    CreatedOn DATETIMEOFFSET(7) NOT NULL DEFAULT SYSDATETIMEOFFSET(),
    LastModifiedOn DATETIMEOFFSET(7) NOT NULL DEFAULT SYSDATETIMEOFFSET()
    CONSTRAINT Courses_Professors FOREIGN KEY (ProfessorId) REFERENCES Professors(Id),
    CONSTRAINT Courses_Semesters FOREIGN KEY (SemesterId) REFERENCES Semesters(Id),
);

INSERT INTO [example-db].dbo.Courses
  (Id, Description, ProfessorId, SemesterId)
  VALUES
    ('POT-101-23', 'Potions - 101; ''23', 1, '2023-2'),
    ('TLP-201-23', 'Telepathy, Advanced; ''23', 2, '2023-2')
;

--Students

CREATE TABLE [example-db].dbo.StudentCourses (
    StudentId INT,
    CourseId varchar(20),
    PRIMARY KEY (StudentId, CourseId),
    FOREIGN KEY (StudentId) REFERENCES Students(Id),
    FOREIGN KEY (CourseId) REFERENCES Courses(Id)
);

INSERT INTO [example-db].dbo.StudentCourses
  VALUES
    (1, 'POT-101-23')
;
