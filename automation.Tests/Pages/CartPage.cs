using Microsoft.Playwright;
using automation.Tests.Support;

namespace automation.Tests.Pages;

public class CartPage
{
    private readonly PlaywrightDriver _driver;

    public CartPage(PlaywrightDriver driver)
    {
        _driver = driver;
    }

    private IPage Page =>
        _driver.Page ?? throw new InvalidOperationException("Browser not initialized");

    public async Task AssertProductIsVisibleAsync(string productName)
    {
        var productHeading = Page.GetByRole(
            AriaRole.Heading,
            new() { Level = 3, Name = productName, Exact = true });

        await Assertions.Expect(productHeading).ToBeVisibleAsync();
    }

    public async Task AssertCartContainsSelectionAsync(
        string productName,
        string size,
        string color)
    {
        await AssertProductIsVisibleAsync(productName);
        await Assertions.Expect(
            Page.GetByText($"Size: {size}", new() { Exact = true }))
            .ToBeVisibleAsync();
        await Assertions.Expect(
            Page.GetByText($"Color: {color}", new() { Exact = true }))
            .ToBeVisibleAsync();
    }
}
