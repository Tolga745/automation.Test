using Microsoft.Playwright;
using automation.Tests.Support;
namespace automation.Tests.Pages;

public class CheckoutPage
{
    private readonly PlaywrightDriver _driver;

    public CheckoutPage(PlaywrightDriver driver)
    {
        _driver = driver;
    }

    private IPage Page => _driver.Page ?? throw new InvalidOperationException("Browser not initialized");

    private ILocator orderNowButton => Page.GetByRole(AriaRole.Button, new() { Name = "Order Now" });
    private ILocator AddreessBox => Page.Locator("textarea[name=\"address\"]");
    private ILocator creditCardBox => Page.GetByRole(AriaRole.Textbox, new() { Name = "5678 9012 3456" });
    private ILocator cardDateBox => Page.GetByRole(AriaRole.Textbox, new() { Name = "MM/YY" });
    private ILocator cardCvvBox => Page.GetByRole(AriaRole.Textbox, new() { Name = "123", Exact = true });
    private ILocator MakePaymentButton => Page.GetByRole(AriaRole.Button, new() { Name = "Make Payment" });
    public ILocator OrderConfirmationMessage => Page.GetByRole(AriaRole.Heading, new() { Name = "Order Confirmed!" });
    public ILocator InvoicePreviewMessage => Page.GetByRole(AriaRole.Heading, new() { Name = "Invoice Preview" });
    public async Task FillAddress(string address)
    {
        await AddreessBox.FillAsync(address);
    }

    public async Task FillCreditCard(string creditCard)
    {
        await creditCardBox.FillAsync(creditCard);
    }

    public async Task FillCardDate(string cardDate)
    {
        await cardDateBox.FillAsync(cardDate);
    }

    public async Task FillCardCvv(string cardCvv)
    {
        await cardCvvBox.FillAsync(cardCvv);
    }

    public async Task ClickOrderNow()
    {
        await orderNowButton.ClickAsync();
    }

    public async Task ClickMakePayment()
    {
        await MakePaymentButton.ClickAsync();
    }

}