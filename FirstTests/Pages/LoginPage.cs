using OpenQA.Selenium;

namespace FirstTests.Pages;

/// <summary>Trang đăng nhập https://www.saucedemo.com — nghiệp vụ: mở trang, đăng nhập, đọc thông báo lỗi.</summary>
public class LoginPage : BasePage
{
    public const string Url = "https://www.saucedemo.com/";

    private readonly By oTenDangNhap = By.Id("user-name");
    private readonly By oMatKhau = By.Id("password");
    private readonly By nutDangNhap = By.Id("login-button");
    private readonly By thongBaoLoi = By.CssSelector("[data-test='error']");

    public LoginPage(IWebDriver driver) : base(driver) { }

    /// <summary>Người dùng mở trang đăng nhập.</summary>
    public LoginPage Mo()
    {
        driver.Navigate().GoToUrl(Url);
        return this;
    }

    /// <summary>Người dùng nhập tài khoản, mật khẩu rồi bấm Login.</summary>
    public InventoryPage DangNhap(string taiKhoan, string matKhau)
    {
        Tim(oTenDangNhap).SendKeys(taiKhoan);
        Tim(oMatKhau).SendKeys(matKhau);
        Tim(nutDangNhap).Click();
        return new InventoryPage(driver);
    }

    /// <summary>Người dùng đọc thông báo lỗi hiện ra sau khi đăng nhập thất bại.</summary>
    public string LayThongBaoLoi() => Tim(thongBaoLoi).Text;

    /// <summary>Người dùng có nhìn thấy thông báo lỗi hay không.</summary>
    public bool ThongBaoLoiDaHienThi() => Tim(thongBaoLoi).Displayed;

    /// <summary>Địa chỉ trang người dùng đang đứng.</summary>
    public string LayUrl() => driver.Url;
}
