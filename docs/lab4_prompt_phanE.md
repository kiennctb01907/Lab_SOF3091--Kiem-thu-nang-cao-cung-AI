Em đang refactor bộ test Selenium C# (NUnit 4.3.2, Selenium.WebDriver 4.52, .NET 10) cho trang https://www.saucedemo.com sang mô hình Page Object Model (POM). Anh/chị refactor giúp em file test bên dưới theo ĐÚNG các quy ước sau:

1. Cấu trúc: thư mục `Pages/` (namespace `FirstTests.Pages`) và `Tests/` (namespace `FirstTests.Tests`).
2. `BasePage` là abstract class chứa: `driver`, `WebDriverWait` (10 giây) và hàm `Tim(By)` tìm phần tử KÈM chờ. Mọi Page class phải kế thừa `BasePage`, không tự khai báo lại `driver`/`wait`.
3. `LoginPage` có tối thiểu: `DangNhap(tk, mk)` (trả về `InventoryPage`) và `LayThongBaoLoi()`. `InventoryPage` có tối thiểu: `DaHienThi()` và các hàm lấy danh sách sản phẩm (tên, giá) mà test cần.
4. Locator để `private readonly By`; hành vi nghiệp vụ để `public`. Tên hành vi phải trả lời "người dùng LÀM ĐƯỢC GÌ" (đặt tên tiếng Việt không dấu), không đặt theo kiểu "trang có thẻ nào".
5. KHÔNG có `Assert` bên trong Page Object. KHÔNG Page Object nào trả về `IWebElement`.
6. Trong Page class dùng `Tim(locator)` thay cho `driver.FindElement(locator)`.
7. File test sau khi refactor KHÔNG còn chữ `By` nào (mở file bấm Ctrl+F gõ By phải không có kết quả); chỉ gọi Page Object rồi `Assert`.
8. Refactor là đổi CẤU TRÚC nhưng GIỮ NGUYÊN hành vi: mọi ca test và mọi Assert của file gốc phải còn đủ, kết quả chạy phải giống hệt trước khi refactor.

Hãy trả về toàn bộ code của `BasePage.cs`, `LoginPage.cs`, `InventoryPage.cs` và `LoginTests.cs` đã refactor.

File test gốc (`FirstTests/LoginTests.cs`):

```csharp
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace FirstTests;

[TestFixture]
public class LoginTests
{
    private const string BaseUrl = "https://www.saucedemo.com/";

    private IWebDriver driver = null!;

    [SetUp]
    public void SetUp()
    {
        var options = new ChromeOptions();
        // Tắt trình quản lý mật khẩu để Chrome không hiện popup cảnh báo mật khẩu sau khi đăng nhập
        options.AddUserProfilePreference("credentials_enable_service", false);
        options.AddUserProfilePreference("profile.password_manager_enabled", false);
        options.AddUserProfilePreference("profile.password_manager_leak_detection", false);

        driver = new ChromeDriver(options);
        driver.Manage().Window.Maximize();
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        driver.Navigate().GoToUrl(BaseUrl);
    }

    [TearDown]
    public void TearDown()
    {
        driver?.Quit();
        driver?.Dispose();
    }

    /// <summary>Hàm dùng chung: nhập username, password và bấm Login.</summary>
    private void Login(string username, string password)
    {
        driver.FindElement(By.Id("user-name")).SendKeys(username);
        driver.FindElement(By.Id("password")).SendKeys(password);
        driver.FindElement(By.Id("login-button")).Click();
    }

    [Test]
    public void Login_StandardUser_ShouldGoToInventoryPage()
    {
        Login("standard_user", "secret_sauce");

        // Chờ danh sách sản phẩm hiện ra rồi mới kiểm tra URL
        driver.FindElement(By.CssSelector("[data-test='inventory-list']"));
        Console.WriteLine($"URL sau khi đăng nhập: {driver.Url}");

        Assert.That(driver.Url, Is.EqualTo(BaseUrl + "inventory.html"),
            "Đăng nhập đúng phải chuyển sang trang inventory.html");
    }

    [Test]
    public void Login_LockedOutUser_ShouldShowErrorMessage()
    {
        Login("locked_out_user", "secret_sauce");

        IWebElement error = driver.FindElement(By.CssSelector("[data-test='error']"));
        Console.WriteLine($"Thông báo lỗi: {error.Text}");

        Assert.Multiple(() =>
        {
            Assert.That(error.Displayed, Is.True, "Thông báo lỗi phải hiện ra");
            Assert.That(error.Text, Is.EqualTo("Epic sadface: Sorry, this user has been locked out."),
                "Nội dung thông báo lỗi không đúng");
            Assert.That(driver.Url, Is.EqualTo(BaseUrl), "Tài khoản bị khóa thì phải ở lại trang đăng nhập");
        });
    }

    [Test]
    public void Inventory_ShouldListSixProductsWithValidNameAndPrice()
    {
        Login("standard_user", "secret_sauce");

        IReadOnlyCollection<IWebElement> items = driver.FindElements(By.CssSelector("[data-test='inventory-item']"));

        var names = new List<string>();
        var prices = new List<string>();
        foreach (IWebElement item in items)
        {
            // Lấy thông tin BÊN TRONG từng sản phẩm bằng item.FindElement
            string name = item.FindElement(By.CssSelector("[data-test='inventory-item-name']")).Text;
            string price = item.FindElement(By.CssSelector("[data-test='inventory-item-price']")).Text;
            Console.WriteLine($"{name} - {price}");
            names.Add(name);
            prices.Add(price);
        }

        Assert.Multiple(() =>
        {
            Assert.That(items, Has.Count.EqualTo(6), "Trang inventory phải có đúng 6 sản phẩm");
            Assert.That(names, Has.None.Empty, "Không được có tên sản phẩm rỗng");
            Assert.That(names, Is.Unique, "Tên sản phẩm không được trùng nhau");
            Assert.That(names, Does.Contain("Sauce Labs Backpack"), "Danh sách phải có sản phẩm Sauce Labs Backpack");
            Assert.That(prices, Has.All.StartWith("$"), "Mọi giá đều phải bắt đầu bằng ký tự $");
            Assert.That(prices.Select(p => decimal.Parse(p.TrimStart('$'), System.Globalization.CultureInfo.InvariantCulture)),
                Has.All.GreaterThan(0m), "Giá sản phẩm phải là số dương");
            Assert.That(prices[names.IndexOf("Sauce Labs Backpack")], Is.EqualTo("$29.99"),
                "Giá của Sauce Labs Backpack phải là $29.99");
        });
    }
}
```
