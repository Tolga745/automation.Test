using Reqnroll;
using automation.Tests.Pages;
using automation.Tests.Pages.Components;
using automation.Tests.Support;
using Microsoft.Playwright;

namespace automation.Tests.StepDefinitions;

[Binding]
public class CheckoutSteps
{
    private readonly CheckoutPage _checkoutPage;
    private readonly PlaywrightDriver _driver;

    public CheckoutSteps(CheckoutPage checkoutPage, PlaywrightDriver driver)
    {
        _checkoutPage = checkoutPage;
        _driver = driver;
    }

    private IPage Page => _driver.Page ?? throw new InvalidOperationException("Browser not initialized");

    [When("I press the order now button")]
    public async Task WhenIPressTheOrderNowButton()
    {
        await _checkoutPage.ClickOrderNow();
    }

    [When("I enter the address as {string}")]
    public async Task WhenIEnterTheAddressAs(string address)
    {
        await _checkoutPage.FillAddress(TestConfiguration.Resolve(address));
    }

    [When("I enter the card number as {string}")]
    public async Task WhenIEnterTheCreditCardAs(string creditCard)
    {
        await _checkoutPage.FillCreditCard(TestConfiguration.Resolve(creditCard));
    }

    [When("I enter the expiration date as {string}")]
    public async Task WhenIEnterTheCardDateAs(string cardDate)
    {
        await _checkoutPage.FillCardDate(TestConfiguration.Resolve(cardDate));
    }

    [When("I enter the CVV as {string}")]
    public async Task WhenIEnterTheCardCvvAs(string cardCvv)
    {
        await _checkoutPage.FillCardCvv(TestConfiguration.Resolve(cardCvv));
    }

    [When("I press the make payment button")]
    public async Task WhenIPressTheMakePaymentButton()
    {
        await _checkoutPage.ClickMakePayment();
    }

    [Then("I should see the order confirmation message {string}")]
    public async Task ThenIShouldSeeTheOrderConfirmationMessage(string message)
    {
        await Assertions.Expect(_checkoutPage.OrderConfirmationMessage).ToBeVisibleAsync();
    }

    [Then("I should see the invoice preview")]
    public async Task ThenIShouldSeeTheInvoicePreview()
    {
        await Assertions.Expect(_checkoutPage.InvoicePreviewMessage).ToBeVisibleAsync();
    }

}