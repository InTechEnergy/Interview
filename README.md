# .NET Interview

Welcome!

This project provides you with a few patterns that our code uses,
and aims to gauge your ability to follow,
and extend these patterns where applicable,
so that the cohesiveness and style of code is maintained.

Please feel free to use whatever editor or IDE you like.  
We work with Visual Studio 2022, Rider, and even Visual Studio Code,
on Windows, macOS, and Linux.

We use, and this project targets, .NET 7 and C# 11.
While we would prefer that you are familiar with, and use features introduced
in recent versions of C#, we treasure far more solid development skills,
a keen mind for unit testing, and an ability to adapt to new patterns.

On the database front, we use Microsoft SQL Server and a section at this end
of this document will help you set it up and create a sample database
for the project to run against.  
We access this database using Entity Framework; we also expect you
to be familiar with good database design practices,
known DDL, and be capable to craft moderately efficient SQL statements.


## Process

We have prepared a list of tasks we expect it'll take a senior dev a few
hours. Please feel free to stop whenever you feel like you've accomplished enough.

Also, if you feel you're running out of time but would like to address some of the
tasks, feel free to open a PR with no code changes, but use the description
of the PR to discuss what approach you would take to implement this task.  
"Task 5" is a good example where you could do this.

We aim to test both your programming skills as well as your ability to work in
the manner we operate, and exhibit mastery of the tools we use.

To that extent, please consider each of the tasks below being a "user story".

We expect that you will:

- fork the repo under your own account and create a branch for each task/"user story"
  as you work on them;
- follow the style, coding conventions and architecture the codebase currently
  exhibits; be able to explain the reason behind deviating when needed;
- make use of existing code, when appropriate, and keep isolation when needed;
- exhibit knowledge of _SOLID_ design principles;
- create commits that represent unit of work and
  that they have good commit messages;
- open a pull request against the original repo for each task;
- ensure that the code in the PR compiles and the tests pass;
- favor unit tests over integration tests.

### Tasks

The tasks are roughly in order of importance. If you feel a task is too complex,
skip it and choose another.

The tasks are occasionally vague on purpose: we would like you to show initiative
and use your experience in providing an acceptable initial solution for them.

#### Task 1 - End-point to list Students and their current Course load

We need an API end-point that produces a list of _Students_ (stored in the `Students` table),
with all their attributes, and for each student listing the number of _Courses_
(not the list but the number!) they are registered for in the **current Semester**.

- You will need to create a new set of domain classes, a new aggregate root in
Domain Driven Design (DDD) parlance, under the
`ExampleApp.Api.Domain.Students` namespace.
- Create the appropriate table structure(s) to connect the `Students` table to
  the `Courses` table, and provide the correct EF mapping in a new _Student_-centered
  database context.  
  Save the DDL for this or these new objects either in `01-create.sql` or to a new file;
  feel free to provide some sample data.
- Create a controller and action that will return this list when invoked.
- Create appropriate `IMediator` queries and query handlers.
- Create unit or integration tests for the controller action.

Notes:

- a _current Semester_ is the semester where the current date falls between its
  start and end dates;
- a _Course_ is a taught by a _Professor_ in a given _Semester_;
- a _Student_ register for one or more _Course_


#### Task 2 - End-point to allow a Student to register for a Course

Building upon the previous task, add an end-point to allow a _Student_ to
register for a _Course_.

- The minimum payload sent to this new end-point
  should have either the _Student_'s `FullName` or their `Badge` "number",
  and the `Id` of the _Course_ they register for.
- Validate that the _Course_ is "current".  
  A _Student_ cannot register for past or future course;
  produce an appropriate error message when this situation occurs.
- Create appropriate `IMediator` queries and query handlers,
  commands and command handlers
- Create unit or integration tests for the controller action.


#### Task 3 - End-point to allow a Student to un-register from a Course

Building upon the previous tasks, add an end-point to allow a _Student_ to
un-register for a _Course_.  
It's entirely up to you if you want to handle this operation as a "soft-" or
"hard-delete".

- The minimum payload sent to this new end-point
  should have either the _Student_'s `FullName` or their `Badge` "number",
  and the `Id` of the _Course_ they registered for.
- Validate that the _Course_ is not "past".  
  A _Student_ cannot un-register from a past course;
  produce an appropriate error message when this situation occurs.
- Create appropriate `IMediator` queries and query handlers,
  commands and command handlers.
- Create unit or integration tests for the controller action.

#### Task 3 - Convert for-each loop in `GetCurrent`

Convert the `foreach` loop present in `CourseController.GetCurrent` into a
"one-liner" LINQ statement, retaining the same structure the method returns.

Notes:

- "one-liner" in this context does not imply a single physical code line or
  a single statement of code, but rather a way to replace the multiple lines
  of the `foreach` loop, and the loop itself, with the equivalent semantics
  provided by LINQ.
- this would effectively turn the `new List` + `foreach` + `return models`
  into a `return courses.....;`

#### Task 4 - Change the format of the listing of current Courses

The `GetCurrent` method of the `CoursesController` produces a JSON response
in the following format:

```json5
[
  {
    "id": "course id 1",
    "description": "course description 1",
    "semester": {
      "key": "sem-key",
      "name": "semester name",
    },
    "professor": {
      "key": "professor id",
      "name": "professor full name"
    }
  },
  {
    "id": "course id 2",
    "description": "course description 2",
    "semester": {
      "key": "sem-key",
      "name": "semester name",
    },
    "professor": {
      "key": "professor id",
      "name": "professor full name"
    }
  },
  // ... similar structure
]
```

Change the format of the response so that it's rooted in the current semester.

For example:

```json5
{
  "semester": {
    "key": "sem-key",
    "name": "semester name",
    "startDate": "yyyy-mm-dd",
    "endDate": "yyyy-mm-dd",
    "courses": [
      {
        "key": "course id 1",
        "name": "course description 1",
        "professor": {
          "key": "professor id",
          "name": "professor full name"
        }
      },
      {
        "key": "course id 2",
        "name": "course description 2",
        "professor": {
          "key": "professor id",
          "name": "professor full name"
        }
      },
      // ... rest of courses for the semester
    ]
  }
}
```

#### Task 5 - In the `Course` class change the name of the `Professor` property to `Lecturer`

In the `Course` class, change the name of the property named `Professor` to `Lecturer`,

The type of this property continues to remain `Professor`, only the property name changes.

Update EF mappings and associated constructor parameters, but do not change any database objects.

#### Task 6 - (Bonus) Bulk-update student registrations

As a new school year rolls by, the admin staff would like to be able to upload
the student registration in bulk.

They have a spreadsheet with a large number of elections.  
The spreadsheet has three columns: 

- Student Name
- Student Badge
- Course Id

Due to the very large size of this data set, EF exhibits poor performance
creating the records.

Provide a technical solution that ensures this large amount of data is inserted
as fast as possible.

Notes:

- the entry-point should still be the API: an end-point that accepts
  an Excel file upload -- or CSV, and which then parses and loads
  the data therein into the database;
- the solution should be entirely contained within the `ExampleApp.Api` project;
  you don't have access to the bulk-load tools provided by `osql` or
  similar command-line tools;
- discuss how this would change if the application was deployed to a cloud provider
  like Azure or AWS: what services would you use then?

## Setting Up

If you already have SQL Server on your computer, you can skip ahead.  
If not, the instructions below provide two paths: SQL Server LocalDB
or a Docker-hosted instance. If you're on macOS or Linux, the latter
is the only possible approach.

### LocalDB

Install [SQL Server Express 2019](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb?view=sql-server-ver16#installation-media),
then use the command line [to create a named instance](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb?view=sql-server-ver16#create-and-connect-to-a-named-instance).
Naming this instance `localdev` will help with the rest of the assignment.

You should be able [to connect to the instance](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb?view=sql-server-ver16#connect-to-a-shared-instance-of-localdb)
from Visual Studio, or Azure Data Studio, or your favorite MSSQL client.

### Docker Container

https://hub.docker.com/r/microsoft/mssql-server

Below are provided shell instructions; the link above has both Windows and Linux instructions.

First create a container using the command line.

Below, change `<your password>` to a decent password.  
You will update the connection string in `appsettings.json` with this value.

```sh
docker run --cap-add SYS_PTRACE -e 'ACCEPT_EULA=1' \
  -e 'MSSQL_SA_PASSWORD=<your strong! password>' \
  -e 'TZ=America/Chicago' \
  -p 1433:1433 \
  --name interview_sql \
  -d mcr.microsoft.com/mssql/server:2022-latest
```

Now you can connect to the dabase using `localhost`, and the `SA` user.

To shut down, but keep the container: `docker stop interview_sql`.  
To restart the container with the creation parameters: `docker start interview_sql`.

### Data structure creation

Use the SQL script in `assets/01-create.sql` to create the database,
tables, as well as populate a small amount of data in those tables.

### Connection strings

The app uses the `Default` connection string.  
The `appsettings.json` file in both the `src/ExampleApp.Api` folder and in the
`tests/ExampleApp.Tests` folder has preset connections for both LocalDB and Docker.
Select whichever suits your set up and update the value of the `Default` key.



