using Reqnroll;
using automation.Tests.Support;
namespace automation.Tests.Hooks;

[Binding]
public class TestHooks
{
    private readonly PlaywrightDriver _driver;
    private readonly ScenarioContext _scenarioContext;

    public TestHooks(
        PlaywrightDriver driver,
        ScenarioContext scenarioContext)
    {
        _driver = driver;
        _scenarioContext = scenarioContext;
    }
    
    [BeforeScenario]
    public async Task BeforeScenario()
    {
        await _driver.StartAsync();
    }
    
    [AfterScenario]
    public async Task AfterScenario()
    {
        if (_scenarioContext.TestError != null && _driver.Page != null)
        {
            await _driver.Page.ScreenshotAsync(new()
            {
                Path = $"failure-{DateTime.Now:yyyyMMdd-HHmmss}.png",
                FullPage = true
            });
        }
        await _driver.DisposeAsync();
    }
}