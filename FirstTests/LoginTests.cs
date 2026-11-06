using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace FirstTests;

/// <summary>
/// Lab 2 - Selenium WebDriver cơ bản trên trang https://www.saucedemo.com
/// Phần B: đăng nhập luồng thuận và luồng nghịch.
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
}
