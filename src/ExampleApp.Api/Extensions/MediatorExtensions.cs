using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using ExampleApp.Api.Utils.Exceptions;
using ExampleApp.Api.Utils.Models;
using MediatR;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace ExampleApp.Api.Extensions;

public static class MediatorExtensions
{
    public static async Task<IResult> SendCommand(this IMediator mediator, IRequest<Result> request)
        => await mediator.Send(request) switch
        {
            (true, _, _) => Results.Ok(),
            var (_, exception, errors) => HandleError(exception!, errors),
        };

    public static async Task<IResult> SendCommand<T>(this IMediator mediator, IRequest<Result<T>> request)
        => await mediator.Send(request) switch
        {
            (true, var result) => Results.Ok(result),
            var (_, _, exception, errors) => HandleError(exception!, errors),
        };

    private static IResult HandleError(Exception exception, List<ResultError> errors)
    {
        var problemDetails = CreateProblemDetails(exception, errors);
        return exception switch
        {
            AppException e => Results.Problem(title: e.Message, type: e.Type, statusCode: 500),
            ValidationException e => Results.BadRequest(problemDetails! with { Type = e.GetType().ToString(), Title = e.Message, Status = 400 }),
            _ => Results.Problem(title: "An error occurred while processing your request", detail: "An error occurred while processing your request, please try again in a few moments"),
        };
    }

    private static ProblemDetails CreateProblemDetails(Exception exception, List<ResultError> errors)
    {
        var errorsDict = new Dictionary<string, string[]>();
        errors.ForEach(e => errorsDict.Add(JsonNamingPolicy.CamelCase.ConvertName(e.Source), new[] { e.Description }));
        var appException = exception?.GetType() == typeof(AppException) ? (AppException)exception : null;
        var problemDetails = new ProblemDetails()
        {
            Title = appException?.Message ?? "An error occurred while processing your request",
            Errors = errorsDict,
        };

        return problemDetails;
    }
}
