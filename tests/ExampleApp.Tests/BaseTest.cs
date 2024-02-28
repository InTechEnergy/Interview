namespace ExampleApp.Tests;

[Collection(nameof(TestApplicationCollection))]
public class BaseTest : IAsyncDisposable
{
    protected TestApplication TestApplication;

    public BaseTest(TestApplication testApplication)
    {
        TestApplication = testApplication;
    }

    public async ValueTask DisposeAsync()
    {
        await TestApplication.DisposeAsync();
    }

    public virtual Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

}
