using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace FirstTests.Lab3;

/// <summary>
/// Lớp cơ sở cho các test Lab 3.
/// Chỉ dùng Explicit Wait (WebDriverWait), KHÔNG bật Implicit Wait để thời gian chờ không bị cộng dồn.
/// </summary>
public abstract class Lab3TestBase
{
    protected IWebDriver driver = null!;
    protected WebDriverWait wait = null!;

    [SetUp]
    public void SetUp()
    {
        var options = new ChromeOptions();
        // Tắt trình quản lý mật khẩu để Chrome không hiện popup sau khi đăng nhập saucedemo
        options.AddUserProfilePreference("credentials_enable_service", false);
        options.AddUserProfilePreference("profile.password_manager_enabled", false);
        options.AddUserProfilePreference("profile.password_manager_leak_detection", false);

        driver = new ChromeDriver(options);
        driver.Manage().Window.Maximize();
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    }

    [TearDown]
    public void TearDown()
    {
        driver?.Quit();
        driver?.Dispose();
    }
}
