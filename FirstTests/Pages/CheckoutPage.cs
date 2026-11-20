using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace FirstTests.Pages;

/// <summary>
/// Trang checkout-step-one.html của saucedemo (Your Information): First Name, Last Name, Zip/Postal Code.
/// Phải đăng nhập trước thì mới mở được trang này.
/// </summary>
public class CheckoutPage : BasePage
{
    private readonly By oHo = By.Id("first-name");
    private readonly By oTen = By.Id("last-name");
    private readonly By oMaBuuChinh = By.Id("postal-code");
    private readonly By nutTiepTuc = By.Id("continue");
    private readonly By thongBaoLoi = By.CssSelector("[data-test='error']");
    private readonly By nutDongLoi = By.CssSelector("[data-test='error-button']");

    public CheckoutPage(IWebDriver driver) : base(driver) { }

    /// <summary>Người dùng (đã đăng nhập) mở trang nhập thông tin giao hàng.</summary>
    public CheckoutPage Mo()
    {
        driver.Navigate().GoToUrl(cfg.BaseUrl + "checkout-step-one.html");
        Tim(oMaBuuChinh);
        return this;
    }

    /// <summary>Người dùng điền First Name, Last Name, Zip/Postal Code.</summary>
    public CheckoutPage DienThongTin(string ho, string ten, string maBuuChinh)
    {
        Tim(oHo).SendKeys(ho);
        Tim(oTen).SendKeys(ten);
        Tim(oMaBuuChinh).SendKeys(maBuuChinh);
        return this;
    }

    /// <summary>Người dùng bấm Continue.</summary>
    public CheckoutPage BamTiepTuc()
    {
        Tim(nutTiepTuc).Click();
        return this;
    }

    /// <summary>Người dùng có sang được bước 2 (Checkout: Overview) hay không. Trả false thay vì ném lỗi.</summary>
    public bool DaSangBuocHai()
    {
        try
        {
            return new WebDriverWait(driver, TimeSpan.FromSeconds(cfg.CheckTimeoutSeconds))
                .Until(d => d.Url.Contains("checkout-step-two"));
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }

    /// <summary>Người dùng có thấy thông báo lỗi hay không.</summary>
    public bool CoThongBaoLoi() => CoHienThi(thongBaoLoi);

    /// <summary>Người dùng đọc thông báo lỗi.</summary>
    public string LayThongBaoLoi() => Tim(thongBaoLoi).Text;

    /// <summary>Người dùng bấm nút X để đóng thông báo lỗi.</summary>
    public CheckoutPage DongThongBaoLoi()
    {
        Tim(nutDongLoi).Click();
        return this;
    }

    /// <summary>Người dùng đọc giá trị đang có trong ô Zip/Postal Code.</summary>
    public string LayMaBuuChinhDangNhap() => Tim(oMaBuuChinh).GetDomProperty("value") ?? string.Empty;
}
