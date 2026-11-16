using OpenQA.Selenium;

namespace FirstTests.Pages;

/// <summary>Trang đăng nhập saucedemo (BaseUrl trong appsettings.json) — nghiệp vụ: đăng nhập, đọc thông báo lỗi.</summary>
public class LoginPage : BasePage
{
    private readonly By oTenDangNhap = By.Id("user-name");
    private readonly By oMatKhau = By.Id("password");
    private readonly By nutDangNhap = By.Id("login-button");
    private readonly By thongBaoLoi = By.CssSelector("[data-test='error']");

    public LoginPage(IWebDriver driver) : base(driver) { }

    /// <summary>Địa chỉ trang đăng nhập, lấy từ config.</summary>
    public string Url => cfg.BaseUrl;

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

    /// <summary>Người dùng có nhìn thấy thông báo lỗi hay không (trả false nếu không có, không ném lỗi).</summary>
    public bool ThongBaoLoiDaHienThi() => CoHienThi(thongBaoLoi);

    /// <summary>Địa chỉ trang người dùng đang đứng.</summary>
    public string LayUrl() => driver.Url;
}
