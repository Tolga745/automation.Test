using Reqnroll;
using automation.Tests.Pages;
using automation.Tests.Pages.Components;
using automation.Tests.Support;
using Microsoft.Playwright;

namespace automation.Tests.StepDefinitions;

[Binding]
public class NavBarSteps
{
    private readonly NavigationBar _navBar;
    private readonly Wishlist _wishlist;
    private readonly CartPage _cartPage;
    private readonly PlaywrightDriver _driver;
    private readonly ScenarioContext _scenarioContext;
    private const string AddedWishlistProductNameKey = "AddedWishlistProductName";
    private const string AddedCartProductNameKey = "AddedCartProductName";

    public NavBarSteps(
        NavigationBar navBar,
        Wishlist wishlist,
        CartPage cartPage,
        PlaywrightDriver driver,
        ScenarioContext scenarioContext)
    {
        _navBar = navBar;
        _wishlist = wishlist;
        _cartPage = cartPage;
        _driver = driver;
        _scenarioContext = scenarioContext;
    }

    private IPage Page => _driver.Page ?? throw new InvalidOperationException("Browser not initialized");

    [Given("I am on {string}")]
    public async Task GivenIAmOnTheHomePage(string expectedUrl)
    {
        await Page.GotoAsync(TestConfiguration.Resolve(expectedUrl));
    }

    [Then("I should be on {string}")]
    public async Task ThenIShouldBeOnTheHomePage(string expectedUrl)
    {
        await Assertions.Expect(Page).ToHaveURLAsync(
            TestConfiguration.Resolve(expectedUrl));
    }

    [When("I enter the cart")]
    public async Task WhenIEnterTheCart()
    {
        await _navBar.EnterCart();
    }

    [When("I go to {string}")]
    public async Task WhenIGoTo(string expectedUrl)
    {
        await Page.GotoAsync(TestConfiguration.Resolve(expectedUrl));
    }

    [When("I enter the wishlist page")]
    public async Task WhenIEnterTheWishlist(){
        await _navBar.EnterWishlist();
    }

    [Then("I should see the added product in my wishlist")]
    public async Task ThenIShouldSeeTheAddedProductInMyWishlist()
    {
        if (!_scenarioContext.TryGetValue(
                AddedWishlistProductNameKey,
                out string productName))
        {
            throw new InvalidOperationException(
                "The catalog product name was not captured before the wishlist action.");
        }

        await _wishlist.AssertProductIsVisibleAsync(productName);
    }

    [Then("I should see the guest cart product in the cart")]
    public async Task ThenIShouldSeeTheGuestCartProductInTheCart()
    {
        if (!_scenarioContext.TryGetValue(
                AddedCartProductNameKey,
                out string productName))
        {
            throw new InvalidOperationException(
                "The cart product name was not captured before adding it.");
        }

        await _cartPage.AssertCartContainsSelectionAsync(
            productName,
            "S",
            "white");
    }
}