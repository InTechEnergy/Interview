namespace ExampleApp.Api.Utils.Exceptions;

public class AppException : Exception
{
    public string? Type { get; protected set; }

    public AppException()
    {
    }

    public AppException(string type, string message)
        : base(message)
    {
        Type = type;
    }
}
