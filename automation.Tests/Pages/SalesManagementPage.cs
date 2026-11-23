using Microsoft.Playwright;
using automation.Tests.Support;
namespace automation.Tests.Pages;

public class SalesManagementPage
{
    private readonly PlaywrightDriver _driver;

    public SalesManagementPage(PlaywrightDriver driver)
    {
        _driver = driver;
    }

     private IPage Page => _driver.Page ?? throw new InvalidOperationException("Browser not initiliazed");
    

    private ILocator ChartsButton => Page.GetByRole(AriaRole.Button, new() { Name = "Charts" });
    private ILocator PriceDiscountButton => Page.GetByRole(AriaRole.Button, new() { Name = "Price & Discount" });
    private ILocator InvoicesButton => Page.GetByRole(AriaRole.Button, new() { Name = "Invoices" });
    private ILocator RefundButton => Page.GetByRole(AriaRole.Button, new() { Name = "Refund" });
    private ILocator ProductDropdown => Page.GetByRole(AriaRole.Combobox);
    private ILocator DiscountSpinbutton => Page.GetByRole(
        AriaRole.Spinbutton,
        new() { Name = "Enter discount rate (%) %" });
    private ILocator NewPriceSpinbutton => Page.GetByRole(
        AriaRole.Spinbutton,
        new() { Name = "Enter new price ($)" });
    private ILocator UpdatePriceButton => Page.GetByRole(
        AriaRole.Button,
        new() { Name = "Update Price" });

    public async Task ClickChartsButtonAsync()
    {
        await ChartsButton.ClickAsync();
    }
    public async Task ClickPriceDiscountButtonAsync()
    {
        await PriceDiscountButton.ClickAsync();
    }

    public async Task ClickInvoicesButtonAsync()
    {
        await InvoicesButton.ClickAsync();
    }
    public async Task ClickRefundButtonAsync()
    {
        await RefundButton.ClickAsync();
    }

    public async Task SelectFirstProductAsync()
    {
        await ProductDropdown.WaitForAsync();
        await Page.WaitForFunctionAsync(
            "() => document.querySelectorAll(\"select option[value]:not([value=''])\").length > 0");

        var firstOptionValue = await ProductDropdown
            .Locator("option[value]:not([value=''])")
            .First
            .GetAttributeAsync("value");

        if (string.IsNullOrWhiteSpace(firstOptionValue))
        {
            throw new InvalidOperationException("The product dropdown has no selectable first option.");
        }

        await ProductDropdown.SelectOptionAsync(firstOptionValue);
    }

    public async Task EnterDiscountAsync(string discount)
    {
        await DiscountSpinbutton.FillAsync(discount);
    }

    public async Task EnterNewPriceAsync(string newPrice)
    {
        await NewPriceSpinbutton.FillAsync(newPrice);
    }

    public async Task ClickUpdatePriceAsync()
    {
        await UpdatePriceButton.ClickAsync();
    }
    
}