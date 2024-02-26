using MediatR;

namespace ExampleApp.Api.Domain.Academia.Queries;

internal record FindProfessorByNamedQuery(string Name) : IRequest<Professor?>;
