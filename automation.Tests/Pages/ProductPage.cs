using Microsoft.Playwright;
using automation.Tests.Support;
using System.Globalization;
using System.Text.RegularExpressions;

namespace automation.Tests.Pages;

public class ProductPage
{
    private readonly PlaywrightDriver _driver;
    public ProductPage(PlaywrightDriver driver)
    {
        _driver = driver;
    }

    private IPage Page => _driver.Page ?? throw new InvalidOperationException("Browser not initialized");

    // Locators
    private ILocator ProductLinks => Page.Locator("a[href*='/product/']:has(h3)");
    private ILocator FirstCatalogProductName => ProductLinks.First.Locator("h3");
    private ILocator AddToWishlistButton => Page.GetByRole(AriaRole.Button, new() { Name = "Add to Wishlist" });
    private ILocator SelectSizeButton => Page.GetByRole(AriaRole.Button, new() { Name = "Please select size" });
    private ILocator DecreaseQuantityButton => Page.GetByRole(AriaRole.Button, new() { Name = "-" });
    private ILocator IncreaseQuantityButton => Page.GetByRole(AriaRole.Button, new() { Name = "+" });
    private ILocator ColorOptions => Page.Locator("div[style*='background-color']");
    private ILocator AddToCartButton => Page.GetByRole(AriaRole.Button, new() { Name = "ADD TO CART" });
    private ILocator PriceSortSelect => Page.Locator("select:has(option[value='asc'])");
    private ILocator ProductPrices => Page.Locator("p").Filter(
        new() { HasTextRegex = new Regex(@"^Price:\s*\$") });

    // Actions
    public async Task ClickProductAsync(int oneBasedIndex)
    {
        await ProductLinks.Nth(oneBasedIndex - 1).ClickAsync();
    }

    public async Task<string> GetFirstCatalogProductNameAsync()
    {
        return (await FirstCatalogProductName.InnerTextAsync()).Trim();
    }

    public async Task AddToWishlistAsync()
    {
        await AddToWishlistButton.ClickAsync();
    }

    public async Task SelectColorAsync(int oneBasedIndex)
    {
        if (oneBasedIndex < 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(oneBasedIndex),
                "Color position must be 1 or greater.");
        }

        await ColorOptions.Nth(oneBasedIndex - 1).ClickAsync();
    }

    public async Task SelectSizeAsync(string size)
    {
        await SelectSizeButton.ClickAsync();
        await Page.GetByText(size, new() { Exact = true }).ClickAsync();
    }

    public async Task IncreaseQuantityAsync(int times)
    {
        for (int i = 0; i < times; i++)
        {
            await IncreaseQuantityButton.ClickAsync();
        }
    }

    public async Task DecreaseQuantityAsync(int times)
    {
        for (int i = 0; i < times; i++)
        {
            await DecreaseQuantityButton.ClickAsync();
        }
    }

    public async Task AddToCartAsync()
    {
        await AddToCartButton.ClickAsync();
    }

    public async Task SortByPriceAsync(string sortDirection)
    {
        var sortValue = sortDirection.ToLowerInvariant() switch
        {
            "low to high" or "ascending" or "asc" => "asc",
            "high to low" or "descending" or "desc" => "desc",
            _ => throw new ArgumentException(
                "Sort direction must be 'low to high' or 'high to low'.",
                nameof(sortDirection))
        };

        await PriceSortSelect.SelectOptionAsync(sortValue);
    }

    public async Task AssertPricesSortedAsync(string sortDirection)
    {
        var priceTexts = await ProductPrices.AllTextContentsAsync();
        var prices = priceTexts.Select(ParsePrice).ToList();

        var isAscending = sortDirection.ToLowerInvariant() switch
        {
            "low to high" or "ascending" or "asc" => true,
            "high to low" or "descending" or "desc" => false,
            _ => throw new ArgumentException(
                "Sort direction must be 'low to high' or 'high to low'.",
                nameof(sortDirection))
        };

        var expectedPrices = isAscending
            ? prices.OrderBy(price => price).ToList()
            : prices.OrderByDescending(price => price).ToList();

        Assert.That(prices, Is.EqualTo(expectedPrices),
            $"Products were not ordered correctly for {sortDirection} price sorting.");
    }

    private static decimal ParsePrice(string priceText)
    {
        var match = Regex.Match(priceText, @"\$\s*([\d,.]+)");
        if (!match.Success)
        {
            throw new InvalidDataException($"Invalid price text: '{priceText}'.");
        }

        var value = match.Groups[1].Value.Replace(",", "");
        return decimal.Parse(
            value,
            NumberStyles.AllowDecimalPoint,
            CultureInfo.InvariantCulture);
    }
}