using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace FirstTests;

/// <summary>
/// Tạo ChromeDriver với cấu hình dùng chung cho mọi test.
/// - Selenium Manager tự tải ChromeDriver khớp phiên bản Chrome đang cài.
/// - Đặt biến môi trường HEADLESS=1 để chạy Chrome không giao diện (ví dụ khi chạy trên máy CI).
/// </summary>
public static class DriverFactory
{
    public static IWebDriver CreateChromeDriver()
    {
        var options = new ChromeOptions();
        options.AddArgument("--start-maximized");
        options.AddArgument("--disable-notifications");

        if (Environment.GetEnvironmentVariable("HEADLESS") == "1")
        {
            options.AddArgument("--headless=new");
            options.AddArgument("--window-size=1920,1080");
        }

        IWebDriver driver = new ChromeDriver(options);
        driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        return driver;
    }
}
