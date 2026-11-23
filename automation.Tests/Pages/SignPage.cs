using Microsoft.Playwright;
using automation.Tests.Support;
namespace automation.Tests.Pages;

public class SignPage
{
    private readonly PlaywrightDriver _driver;
    public SignPage(PlaywrightDriver driver)
    {
        _driver = driver;
    }
    private IPage page => _driver.Page ?? throw new InvalidOperationException("Browser not initiliazed");
    
    //Locators
    private ILocator emailBox => page.GetByRole(AriaRole.Textbox, new() { Name = "Email:" });
    private ILocator passwordBox => page.GetByRole(AriaRole.Textbox, new() { Name = "Password:" , Exact = true });
    private ILocator confirmPasswordBox => page.GetByRole(AriaRole.Textbox, new() { Name = "Confirm Password:" });
    private ILocator signInButton => page.GetByRole(AriaRole.Button, new() { Name = "Sign In" });
    private ILocator signUpButton => page.GetByRole(AriaRole.Button, new() { Name = "Sign Up" });

    //Actions
    public async Task FillEmail(string email)
    {
        await emailBox.FillAsync(email);
    }
    public async Task FillPassword(string password)
    {
        await passwordBox.FillAsync(password);
    }
    public async Task FillConfirmationPassword(string password)
    {
        await confirmPasswordBox.FillAsync(password);
    }
    public async Task ClickSignIn()
    {
        await signInButton.ClickAsync();
    }

    public async Task ClickSignUp()
    {
        await signUpButton.ClickAsync();
    }
}