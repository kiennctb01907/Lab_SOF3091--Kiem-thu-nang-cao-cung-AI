using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace FirstTests;

/// <summary>
/// Lab 2 - Selenium WebDriver cơ bản trên trang https://www.saucedemo.com
/// Phần B: đăng nhập luồng thuận và luồng nghịch.
/// Phần C: thu thập tên và giá 6 sản phẩm sau khi đăng nhập.
/// </summary>
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

    // ---------------- Phần B ----------------

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

    // ---------------- Phần C ----------------

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
