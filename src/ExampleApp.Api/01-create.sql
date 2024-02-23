IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Professors] (
    [Id] int NOT NULL IDENTITY,
    [FullName] nvarchar(max) NOT NULL,
    [Extension] nvarchar(max) NULL,
    CONSTRAINT [PK_Professors] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Semesters] (
    [Id] nvarchar(450) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [StartDate] date NOT NULL,
    [EndDate] date NOT NULL,
    CONSTRAINT [PK_Semesters] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Students] (
    [Id] int NOT NULL IDENTITY,
    [FullName] nvarchar(max) NOT NULL,
    [Badge] nvarchar(max) NOT NULL,
    [ResidentStatus] int NOT NULL,
    CONSTRAINT [PK_Students] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Courses] (
    [Id] nvarchar(450) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [ProfessorId] int NOT NULL,
    [SemesterId] nvarchar(450) NULL,
    [CreatedOn] datetimeoffset NOT NULL,
    [LastModifiedOn] datetimeoffset NOT NULL,
    CONSTRAINT [PK_Courses] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Courses_Professors_ProfessorId] FOREIGN KEY ([ProfessorId]) REFERENCES [Professors] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Courses_Semesters_SemesterId] FOREIGN KEY ([SemesterId]) REFERENCES [Semesters] ([Id])
);
GO

CREATE TABLE [StudentCourses] (
    [Id] int NOT NULL IDENTITY,
    [StudentId] int NOT NULL,
    [CourseId] nvarchar(450) NULL,
    CONSTRAINT [PK_StudentCourses] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_StudentCourses_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]),
    CONSTRAINT [FK_StudentCourses_Students_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Students] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Courses_ProfessorId] ON [Courses] ([ProfessorId]);
GO

CREATE INDEX [IX_Courses_SemesterId] ON [Courses] ([SemesterId]);
GO

CREATE INDEX [IX_StudentCourses_CourseId] ON [StudentCourses] ([CourseId]);
GO

CREATE INDEX [IX_StudentCourses_StudentId] ON [StudentCourses] ([StudentId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240223214934_CreateStudentCourse', N'7.0.13');
GO

COMMIT;
GO

