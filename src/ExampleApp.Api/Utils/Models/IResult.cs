namespace ExampleApp.Api.Utils.Models;

public interface IResult
{
    Exception? Exception { get; }

    bool IsSuccess { get; }

    List<ResultError> Errors { get; }

    void AddError(ResultError error);

    void SetException(Exception exception);
}
