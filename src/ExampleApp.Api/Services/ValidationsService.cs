using ExampleApp.Api.Common;
using ExampleApp.Api.Controllers.Models;
using ExampleApp.Api.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ExampleApp.Api.Services;

public class ValidationsService : IValidationsService
{
    public List<ModelStateError> CheckErrorsAsync<T>(T? request, ModelStateDictionary modelState)
    {
        if (request is null)
        {
            return new() { new ModelStateError() { FieldName = "Body", ErrorMessage = new[] { Constants.BODY_REQUEST_IS_REQUIRED } } };
        }

        return !modelState.IsValid
            ? modelState
                .Where(x => x.Value?.Errors.Count > 0)
                .Select(x => new ModelStateError()
                {
                    FieldName = x.Key,
                    ErrorMessage = x.Value?.Errors.Select(e => e.ErrorMessage)
                })
            .ToList()
            : (List<ModelStateError>)new();
    }
}
