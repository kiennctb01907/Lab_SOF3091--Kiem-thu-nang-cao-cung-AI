**[Vai trò]** Bạn là lập trình viên kiểm thử tự động C# nhiều kinh nghiệm, đang viết thêm một Page Object cho framework Selenium của một nhóm sinh viên. Code của bạn sẽ được review kỹ trước khi đưa vào dự án.

**[Ngữ cảnh framework]** Dự án C# .NET 10, NUnit 4, Selenium.WebDriver 4.52, kiểm thử https://www.saucedemo.com theo Page Object Model. Quy ước bắt buộc của dự án:
1. Mọi Page kế thừa `BasePage` (namespace `FirstTests.Pages`), nhận `IWebDriver` qua constructor. Page KHÔNG tự tạo driver (`new ChromeDriver()`...) — driver do `BaseTest` tạo.
2. Tìm phần tử bằng `Tim(locator)` (có chờ); hỏi CÓ/KHÔNG bằng `CoHienThi(locator)`. Không dùng `driver.FindElement` trực tiếp cho thao tác thông thường.
3. Locator để `private readonly By`; hàm public đặt tên theo hành vi của người dùng (tiếng Việt không dấu), không trả về `IWebElement`, không có `Assert` trong Page.
4. Hành vi chuyển trang trả về Page mới; ở lại trang trả về `this`. Tái dùng Page có sẵn: `InventoryPage` (trang sản phẩm), `CheckoutPage` (trang Your Information).
5. TUYỆT ĐỐI không dùng `Thread.Sleep`.

Hai file mẫu của dự án để bạn bắt chước phong cách:

`Pages/BasePage.cs`
```csharp
using FirstTests.Config;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace FirstTests.Pages;

/// <summary>
/// Lớp cha của mọi Page Object: giữ driver, cấu hình, WebDriverWait và hàm Tim(By) tìm phần tử KÈM chờ (Explicit Wait).
/// Thời gian chờ đọc từ TimeoutSeconds trong appsettings.json.
/// </summary>
public abstract class BasePage
{
    protected readonly IWebDriver driver;
    protected readonly TestConfig cfg;
    protected readonly WebDriverWait wait;

    protected BasePage(IWebDriver driver)
    {
        this.driver = driver;
        cfg = ConfigReader.Get();
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(cfg.TimeoutSeconds));
    }

    /// <summary>Tìm 1 phần tử, chờ tối đa TimeoutSeconds cho đến khi nó xuất hiện trong DOM.</summary>
    protected IWebElement Tim(By locator) => wait.Until(d => d.FindElement(locator));

    /// <summary>
    /// Hỏi CÓ/KHÔNG: phần tử có hiện ra trong CheckTimeoutSeconds giây không.
    /// Dùng FindElements (trả danh sách rỗng thay vì ném lỗi) và bắt WebDriverTimeoutException để trả false.
    /// </summary>
    protected bool CoHienThi(By locator)
    {
        var choNgan = new WebDriverWait(driver, TimeSpan.FromSeconds(cfg.CheckTimeoutSeconds));
        try
        {
            return choNgan.Until(d => d.FindElements(locator).Any(e => e.Displayed));
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }
}
```

`Pages/LoginPage.cs`
```csharp
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
```

**[Yêu cầu]** Viết MỘT file mới `Pages/CartPage.cs` cho trang giỏ hàng `cart.html` của saucedemo, có các hành vi:
- Mở trang giỏ hàng bằng cách bấm biểu tượng giỏ ở góc trên (dùng được từ bất kỳ trang nào sau khi đăng nhập).
- Đọc danh sách tên các sản phẩm đang có trong giỏ, đọc giá và số lượng của từng sản phẩm.
- Đọc số trên huy hiệu giỏ hàng (trả về 0 nếu không có huy hiệu).
- Xóa một sản phẩm khỏi giỏ theo tên sản phẩm.
- Bấm "Continue Shopping" (quay về trang sản phẩm) và bấm "Checkout" (sang trang Your Information).

**[Định dạng đầu ra]** Trả về toàn bộ code của `Pages/CartPage.cs` trong một khối ```csharp. Sau khối code, liệt kê ngắn gọn các locator bạn đã dùng và lý do chọn.
