using Microsoft.Playwright;
using automation.Tests.Support;
namespace automation.Tests.Pages.Components;

public class NavigationBar
{
    private readonly PlaywrightDriver _driver;
    public NavigationBar(PlaywrightDriver driver)
    {
        _driver = driver;
    }
    private IPage page => _driver.Page ?? throw new InvalidOperationException("Browser not initiliazed");

    //Locators
    private ILocator NewArrivalsLink => page.GetByRole(AriaRole.Link, new() { Name = "New Arrivals" });
    private ILocator MenLink=> page.GetByRole(AriaRole.Link, new() { Name = "Men", Exact = true });
    private ILocator WomenLink=> page.GetByRole(AriaRole.Link, new() { Name = "Women" });
    private ILocator BestLink=> page.GetByRole(AriaRole.Link, new() { Name = "Best Sellers" });
    private ILocator UserAccountLink=> page.GetByRole(AriaRole.Button, new() { Name = "User Account" });
    private ILocator WishlistLink=> page.GetByRole(
        AriaRole.Button,
        new() { Name = "Wishlist", Exact = true });
    private ILocator CartLink=> page.GetByRole(AriaRole.Button, new() { Name = "Shopping Cart" });
    private ILocator SignInLink =>page.GetByRole(AriaRole.Link, new() { Name = "Sign in" });
    private ILocator SignUpLink=> page.GetByRole(AriaRole.Link, new() { Name = "Sign up" });
    private ILocator SignOutLink=> page.GetByRole(AriaRole.Button, new() { Name = "Sign Out" });
    private ILocator MyAccount=> page.GetByRole(AriaRole.Link, new() { Name = "My Account" });

    //Actions
    public async Task NavigateAsync()
    {
        await page.GotoAsync("https://tolga745.github.io/");
    }

    public async Task EnterNewArrivalsAsync()
    {
        await NewArrivalsLink.ClickAsync();


    }
    public async Task EnterMenAsync()
    {
        await MenLink.ClickAsync();
    }
    public async Task EnterWomenAsync()
    {
        await WomenLink.ClickAsync();
    }
    public async Task EnterBestAsync()
    {
        await BestLink.ClickAsync();
    }
    public async Task EnterSignIn()
    {
        await UserAccountLink.HoverAsync();
        await SignInLink.ClickAsync();
    }
    public async Task EnterSignUp()
    {
        await UserAccountLink.HoverAsync();
        await SignUpLink.ClickAsync();
    }
    public async Task EnterMyAccount()
    {
        await UserAccountLink.HoverAsync();
        await MyAccount.ClickAsync();
    }

    public async Task EnterSignOut()
    {
        await UserAccountLink.HoverAsync();
        await SignOutLink.ClickAsync();
    }

    public async Task EnterWishlist()
    {
        await WishlistLink.ClickAsync();
    }

    public async Task EnterCart()
    {
        await CartLink.ClickAsync();
    }
}