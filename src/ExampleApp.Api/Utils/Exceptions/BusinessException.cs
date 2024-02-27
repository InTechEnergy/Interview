namespace ExampleApp.Api.Utils.Exceptions;

public class BusinessException : AppException
{
    public BusinessException(string type, string message) : base(type: type, message: message)
    {
    }
}
