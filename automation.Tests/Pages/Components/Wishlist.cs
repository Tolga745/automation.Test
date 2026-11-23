using Microsoft.Playwright;
using automation.Tests.Support;
namespace automation.Tests.Pages.Components;

public class Wishlist
{
    private readonly PlaywrightDriver _driver;
    public Wishlist(PlaywrightDriver driver)
    {
        _driver = driver;
    }

    private IPage Page => _driver.Page ?? throw new InvalidOperationException("Browser not initialized");

    public async Task AssertProductIsVisibleAsync(string productName)
    {
        var productHeading = Page.GetByRole(
            AriaRole.Heading,
            new() { Level = 3, Name = productName, Exact = true });

        await Assertions.Expect(productHeading).ToBeVisibleAsync();
    }
    
}