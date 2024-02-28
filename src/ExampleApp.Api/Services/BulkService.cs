using ExampleApp.Api.Controllers.Models;
using ExampleApp.Api.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ExampleApp.Api.Services;

public class BulkService : IBulkService
{
    private readonly string _connectionString;

    public BulkService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default");
    }

    public async Task BulkInsertStudents(List<StudentEnrollmentCourseBulkRequestModel> students)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        var table = new DataTable();
        _ = table.Columns.Add("StudentId", typeof(int));
        _ = table.Columns.Add("CourseId", typeof(string));

        foreach (var student in students)
        {
            int? studentId = await GetStudentId(connection, student.StudentName, student.StudentBadge);

            if (studentId == null)
            {
                continue;
            }

            DataRow row = table.NewRow();
            row["StudentId"] = studentId.Value;
            row["CourseId"] = student.CourseId.ToString();
            table.Rows.Add(row);
        }

        int i = 0;
        while (i < table.Rows.Count)
        {
            using var transaction = connection.BeginTransaction();
            using var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction);
            bulkCopy.DestinationTableName = "StudentCourses";
            _ = bulkCopy.ColumnMappings.Add("StudentId", "StudentId");
            _ = bulkCopy.ColumnMappings.Add("CourseId", "CourseId");

            try
            {
                // Write a batch of records to the database
                var batch = table.AsEnumerable().Skip(i).Take(100).CopyToDataTable();
                await bulkCopy.WriteToServerAsync(batch);

                // If no exception was thrown, commit the transaction
                transaction.Commit();
                i += 100;
            }
            catch (SqlException)
            {
                // If an exception was thrown, roll back the transaction and remove the problematic record
                transaction.Rollback();
                table.Rows.RemoveAt(i);
            }
        }
    }

    private static async Task<int?> GetStudentId(SqlConnection connection, string studentName, string studentBadge)
    {
        var command = new SqlCommand("SELECT Id FROM Students WHERE LOWER(FullName) = LOWER(@StudentName) AND LOWER(Badge) = LOWER(@StudentBadge)", connection);
        _ = command.Parameters.AddWithValue("@StudentName", studentName);
        _ = command.Parameters.AddWithValue("@StudentBadge", studentBadge);

        object? result = await command.ExecuteScalarAsync();
        return result == null ? null : Convert.ToInt32(result);
    }
}
