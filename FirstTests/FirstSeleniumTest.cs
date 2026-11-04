using OpenQA.Selenium;

namespace FirstTests;

/// <summary>
/// Lab 1 - Phần B: Kịch bản Selenium đầu tiên.
/// Mở Chrome -> điều hướng đến trang web -> đọc và in tiêu đề -> Assert -> đóng trình duyệt bằng Quit().
/// </summary>
[TestFixture]
public class FirstSeleniumTest
{
    private IWebDriver driver = null!;

    [SetUp]
    public void SetUp()
    {
        // Khởi tạo ChromeDriver qua DriverFactory (cấu hình ChromeOptions dùng chung).
        driver = DriverFactory.CreateChromeDriver();
    }

    [Test]
    public void OpenSeleniumHomePage_TitleShouldContainSelenium()
    {
        // 1. Điều hướng đến trang web
        driver.Navigate().GoToUrl("https://www.selenium.dev/");

        // 2. Đọc và in tiêu đề trang
        string title = driver.Title;
        Console.WriteLine($"Tiêu đề trang: {title}");

        // 3. Assert kết quả mong đợi
        Assert.That(title, Does.Contain("Selenium"), "Tiêu đề trang phải chứa chữ 'Selenium'");
        Assert.That(driver.Url, Does.StartWith("https://www.selenium.dev"));
    }

    [TearDown]
    public void TearDown()
    {
        // 4. Đóng trình duyệt và kết thúc phiên ChromeDriver
        driver?.Quit();
        driver?.Dispose();
    }
}
