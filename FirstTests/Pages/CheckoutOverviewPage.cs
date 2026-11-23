using System.Globalization;
using OpenQA.Selenium;

namespace FirstTests.Pages;

/// <summary>
/// Lab 7: trang Checkout: Overview (checkout-step-two.html) và trang hoàn tất (checkout-complete.html).
/// Locator đã kiểm trên trang thật bằng test thăm dò: subtotal-label, tax-label, total-label, finish, cancel, complete-header.
/// </summary>
public class CheckoutOverviewPage : BasePage
{
    private readonly By dongSanPham = By.CssSelector("[data-test='inventory-item']");
    private readonly By tienHang = By.CssSelector("[data-test='subtotal-label']");
    private readonly By tienThue = By.CssSelector("[data-test='tax-label']");
    private readonly By tongTien = By.CssSelector("[data-test='total-label']");
    private readonly By nutFinish = By.Id("finish");
    private readonly By nutCancel = By.Id("cancel");
    private readonly By tieuDeHoanTat = By.CssSelector("[data-test='complete-header']");

    public CheckoutOverviewPage(IWebDriver driver) : base(driver) { }

    /// <summary>Người dùng đếm số dòng sản phẩm trên trang Overview.</summary>
    public int DemSoSanPham()
    {
        Tim(tongTien);
        return driver.FindElements(dongSanPham).Count;
    }

    /// <summary>Tiền hàng (Item total), thuế (Tax), tổng (Total) dạng số, đọc từ chữ "Item total: $29.99".</summary>
    public decimal LayTienHang() => DocSoTien(tienHang);
    public decimal LayTienThue() => DocSoTien(tienThue);
    public decimal LayTongTien() => DocSoTien(tongTien);

    /// <summary>Người dùng bấm Finish để hoàn tất đơn.</summary>
    public CheckoutOverviewPage BamFinish()
    {
        Tim(nutFinish).Click();
        return this;
    }

    /// <summary>Người dùng bấm Cancel ở trang Overview: quay về trang sản phẩm.</summary>
    public InventoryPage BamCancel()
    {
        Tim(nutCancel).Click();
        return new InventoryPage(driver);
    }

    /// <summary>Người dùng đọc dòng tiêu đề ở trang hoàn tất (vd "Thank you for your order!").</summary>
    public string LayTieuDeHoanTat() => Tim(tieuDeHoanTat).Text;

    /// <summary>Địa chỉ trang người dùng đang đứng.</summary>
    public string LayUrl() => driver.Url;

    private decimal DocSoTien(By locator)
    {
        string chu = Tim(locator).Text;
        return decimal.Parse(chu[(chu.IndexOf('$') + 1)..], CultureInfo.InvariantCulture);
    }
}
