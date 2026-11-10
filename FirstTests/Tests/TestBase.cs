using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace FirstTests.Tests;

/// <summary>
/// Lớp cơ sở cho mọi file test: mở/đóng Chrome và chụp ảnh khi test thất bại (Lab 3 - Phần D).
/// Chỉ dùng Explicit Wait (nằm trong BasePage), KHÔNG bật Implicit Wait để thời gian chờ không bị cộng dồn.
/// Test chỉ làm việc với Page Object, không tìm phần tử trực tiếp.
/// </summary>
public abstract class TestBase
{
    protected IWebDriver driver = null!;

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
    }

    [TearDown]
    public void TearDown()
    {
        // CHỈ chụp ảnh khi test thất bại, đặt tên file theo tên ca test
        if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed && driver != null)
        {
            string dir = Path.Combine(TestContext.CurrentContext.WorkDirectory, "FailureScreenshots");
            Directory.CreateDirectory(dir);
            string path = Path.Combine(dir, $"{TestContext.CurrentContext.Test.Name}.png");

            Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            screenshot.SaveAsFile(path);
            TestContext.AddTestAttachment(path, "Ảnh chụp màn hình lúc test thất bại");
            Console.WriteLine($"Đã chụp ảnh khi test rớt: {path}");
        }

        driver?.Quit();
        driver?.Dispose();
    }
}
