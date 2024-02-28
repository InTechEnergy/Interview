using CsvHelper;
using ExampleApp.Api.Interfaces;
using System.Globalization;

namespace ExampleApp.Api.Services;

public class CsvFileProcessorService : IFileProcessorService
{
    public bool CanProcess(string contentType, string extension)
    {
        return contentType == "text/csv" && extension == ".csv";
    }

    public List<T> Process<T>(IFormFile file) where T : class, new()
    {
        using var reader = new StreamReader(file.OpenReadStream());
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        return csv.GetRecords<T>().ToList();
    }
}
