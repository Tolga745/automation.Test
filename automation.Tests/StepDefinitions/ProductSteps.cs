using Reqnroll;
using automation.Tests.Pages;
using automation.Tests.Pages.Components;
using automation.Tests.Support;
using Microsoft.Playwright;

namespace automation.Tests.StepDefinitions;

[Binding]
public class ProductSteps
{
    private readonly ProductPage _productPage;
    private readonly PlaywrightDriver _driver;
    private readonly ScenarioContext _scenarioContext;
    private const string AddedWishlistProductNameKey = "AddedWishlistProductName";
    private const string AddedCartProductNameKey = "AddedCartProductName";

    public ProductSteps(
        ProductPage productPage,
        PlaywrightDriver driver,
        ScenarioContext scenarioContext)
    {
        _productPage = productPage;
        _driver = driver;
        _scenarioContext = scenarioContext;
    }

    private IPage Page => _driver.Page ?? throw new InvalidOperationException("Browser not initialized");

    [When("I click the {int} st product")]
    public async Task WhenIClickOnTheProduct(int productIndex)
    {
        if (productIndex == 1)
        {
            var productName = await _productPage.GetFirstCatalogProductNameAsync();
            _scenarioContext[AddedWishlistProductNameKey] = productName;
            _scenarioContext[AddedCartProductNameKey] = productName;
        }

        await _productPage.ClickProductAsync(productIndex);
    }

    [When("I selected the size as {string}")]
    public async Task WhenISelectedTheSizeAs(string size)
    {
        await _productPage.SelectSizeAsync(size);
    }

    [When("I selected the color as {int}")]
    public async Task WhenISelectedTheColorAs(int colorIndex)
    {
        await _productPage.SelectColorAsync(colorIndex);
    }
    
    [When("I press the add to cart button")]
    public async Task WhenIPressTheAddToCartButton()
    {
        await _productPage.AddToCartAsync();
    }

    [When("I press the add to wishlist button")]
    public async Task WhenIPressTheAddToWishlistButton()
    {
        await _productPage.AddToWishlistAsync();
    }

    [When("I order the products by price {string}")]
    public async Task WhenIOrderTheProductsByPrice(string sortDirection)
    {
        await _productPage.SortByPriceAsync(sortDirection);
    }

    [Then("the products should be ordered by price {string}")]
    public async Task ThenTheProductsShouldBeOrderedByPrice(string sortDirection)
    {
        await _productPage.AssertPricesSortedAsync(sortDirection);
    }

    [Then("I should see a message {string}")]
    public async Task ThenIShouldSeeAMessage(string message)
    {
        await Assertions.Expect(Page.GetByText(message, new() { Exact = true })).ToBeVisibleAsync();
    }



}