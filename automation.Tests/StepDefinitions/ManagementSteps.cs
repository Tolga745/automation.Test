using Reqnroll;
using automation.Tests.Pages;
using automation.Tests.Pages.Components;
using automation.Tests.Support;
using Microsoft.Playwright;

namespace automation.Tests.StepDefinitions;

[Binding]
public class ManagementSteps
{
    private readonly SalesManagementPage  _salesManagementPage;
    private readonly PlaywrightDriver _driver;

    public ManagementSteps(SalesManagementPage salesManagementPage, PlaywrightDriver driver)
    {
        _salesManagementPage = salesManagementPage;
        _driver = driver;
    }

    private IPage Page => _driver.Page ?? throw new InvalidOperationException("Browser not initialized");

    [When("I click the price & discount button")]
    public async Task WhenIClickThePriceAndDiscountButton()
    {
        await _salesManagementPage.ClickPriceDiscountButtonAsync();
    }

    [When("I select the first product")]
    public async Task WhenISelectTheFirstProduct()
    {
        await _salesManagementPage.SelectFirstProductAsync();
    }

    [When("I enter the discount as {string}")]
    public async Task WhenIEnterTheDiscountAs(string discount)
    {
        await _salesManagementPage.EnterDiscountAsync(
            TestConfiguration.Resolve(discount));
    }

    [When("I press the update discount button")]
    public async Task WhenIPressTheUpdateDiscountButton()
    {
        await _salesManagementPage.ClickUpdatePriceAsync();
    }

}