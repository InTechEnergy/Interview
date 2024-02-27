namespace ExampleApp.Api.Domain.SharedKernel.Contracts;

public interface IAuditable
{
    public DateTime CreatedOn { get; set; }

    public DateTime ChangedOn { get; set; }

    public bool IsDeleted { get; set; }
}
