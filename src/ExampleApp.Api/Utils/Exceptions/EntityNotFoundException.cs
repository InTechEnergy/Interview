namespace ExampleApp.Api.Utils.Exceptions;

public class EntityNotFoundException : AppException
{
    public EntityNotFoundException()
        : base(type: "Invalid entity", message: "Entity not found")
    {
    }
}
