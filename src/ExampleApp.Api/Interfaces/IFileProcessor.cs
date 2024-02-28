namespace ExampleApp.Api.Interfaces;

public interface IFileProcessorService
{
    public bool CanProcess(string contentType, string extension);
    public List<T> Process<T>(IFormFile file) where T : class, new();
}
