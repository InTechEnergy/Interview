using ExampleApp.Api.Controllers.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ExampleApp.Api.Interfaces;

public interface IValidationsService
{
    public List<ModelStateError> CheckErrorsAsync<T>(T request, ModelStateDictionary modelState);
}
