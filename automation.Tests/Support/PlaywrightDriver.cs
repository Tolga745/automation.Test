using Microsoft.Playwright;
namespace automation.Tests.Support;

public sealed class PlaywrightDriver : IAsyncDisposable
{
    private IPlaywright? _playwright;
    public IBrowser? Browser {get; private set;}
    public IBrowserContext? Context {get; private set;}
    public IPage? Page {get; private set;}

    public string BaseUrl {get; set;} =
        TestConfiguration.Resolve("BASE_URL");

    public async Task StartAsync()
    {
        _playwright = await Playwright.CreateAsync();
        var headlessValue = Environment.GetEnvironmentVariable("PLAYWRIGHT_HEADLESS");
        var headless = !bool.TryParse(headlessValue, out var parsed) || parsed;

        Browser = await _playwright.Chromium.LaunchAsync(
            new BrowserTypeLaunchOptions
            {
                Headless = headless
            });
        
        Context = await Browser.NewContextAsync(
            new BrowserNewContextOptions
            {
                ViewportSize = new ViewportSize
                {
                    Width = 1920,
                    Height = 1080
                },
                IgnoreHTTPSErrors = false
            });
        Page = await Context.NewPageAsync();
    }

    public async ValueTask DisposeAsync()
    {
        if(Context is not null)
        {
            await Context.CloseAsync();
        }

        if(Browser is not null)
        {
            await Browser.CloseAsync();
        }

        _playwright?.Dispose();
    }
}