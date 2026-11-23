using Reqnroll;
using automation.Tests.Pages;
using automation.Tests.Pages.Components;
using automation.Tests.Support;
using Microsoft.Playwright;

namespace automation.Tests.StepDefinitions;

[Binding]
public class SignSteps
{
    private readonly SignPage _signPage;
    private readonly NavigationBar _navBar;
    private readonly PlaywrightDriver _driver;

    public SignSteps(SignPage signPage, NavigationBar navBar, PlaywrightDriver driver)
    {
        _signPage = signPage;
        _navBar = navBar;
        _driver = driver;
    }

    private IPage Page => _driver.Page ?? throw new InvalidOperationException("Browser not initialized");

    [Given("I am on the home page")]
    public async Task GivenIAmOnTheHomePage()
    {
        await _navBar.NavigateAsync();
    }


    [When("I click the sign in link")]
    public async Task ThenIClickTheSignInLink()
    {
        await _navBar.EnterSignIn();
    }

    [When("I click the sign up link")]
    public async Task WhenIClickTheSignUplink()
    {
        await _navBar.EnterSignUp();
    }

    [When("I enter email as {string}")]
    public async Task WhenIEnterTheEmailAs(string email)
    {
        await _signPage.FillEmail(TestConfiguration.Resolve(email));
    }

    [When("I enter the password as {string}")]
    public async Task WhenIEnterThePasswordAs(string password)
    {
        await _signPage.FillPassword(TestConfiguration.Resolve(password));
    }

    [When("I enter the password confirmation as {string}")]
    public async Task WhenIEnterThePasswordConfirmationAs(string password)
    {
        await _signPage.FillConfirmationPassword(
            TestConfiguration.Resolve(password));
    }

    [When("I press the sign in button")]
    public async Task WhenIPressTheSignInButton()
    {
        await _signPage.ClickSignIn();
    }

    [When("I press the sign up button")]
    public async Task WhenIPressTheSignUpButton()
    {
        await _signPage.ClickSignUp();
    }

    

    [Then("I should be on the main page")]
    public async Task ThenIShouldBeOnTheMainPage()
    {
        await Assertions.Expect(Page).ToHaveURLAsync("https://tolga745.github.io/");
    }

    [Then("I should see an error message {string}")]
    public async Task ThenIShouldSeeAnErrorMessage(string expectedErrorMessage)
    {
        await Assertions.Expect(Page.GetByText(expectedErrorMessage)).ToBeVisibleAsync();
    }
}