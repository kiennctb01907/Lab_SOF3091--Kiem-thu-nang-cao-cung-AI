using System.Drawing;
using FirstTests.Config;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

namespace FirstTests.Core;

/// <summary>
/// Tạo trình duyệt theo khóa Browser trong appsettings.json.
/// Create trả về IWebDriver nên phía gọi không cần biết đang chạy Chrome, Firefox hay Edge.
/// Selenium Manager (Selenium 4.6+) tự tải driver phù hợp, không cần WebDriverManager.
/// </summary>
public static class DriverFactory
{
    public static IWebDriver Create(TestConfig cfg)
    {
        string tenTrinhDuyet = cfg.Browser?.Trim() ?? string.Empty;

        IWebDriver driver = tenTrinhDuyet.ToLowerInvariant() switch
        {
            "chrome" => new ChromeDriver(ChromeOpts(cfg)),
            "firefox" => new FirefoxDriver(FirefoxOpts(cfg)),
            "edge" => new EdgeDriver(EdgeOpts(cfg)),
            _ => throw new NotSupportedException(
                $"Trình duyệt \"{cfg.Browser}\" không được hỗ trợ. " +
                "Khóa Browser trong appsettings.json chỉ nhận: Chrome, Firefox, Edge.")
        };

        // Đặt kích thước tường minh thay vì Maximize(): ở chế độ headless cửa sổ là ảo, Maximize() không có tác dụng
        driver.Manage().Window.Size = new Size(cfg.WindowWidth, cfg.WindowHeight);
        return driver;
    }

    private static ChromeOptions ChromeOpts(TestConfig cfg)
    {
        var options = new ChromeOptions();
        if (cfg.Headless)
        {
            options.AddArgument("--headless=new");
        }
        options.AddArgument($"--window-size={cfg.WindowWidth},{cfg.WindowHeight}");

        // Tắt trình quản lý mật khẩu để Chrome không hiện popup sau khi đăng nhập saucedemo
        options.AddUserProfilePreference("credentials_enable_service", false);
        options.AddUserProfilePreference("profile.password_manager_enabled", false);
        options.AddUserProfilePreference("profile.password_manager_leak_detection", false);
        return options;
    }

    private static EdgeOptions EdgeOpts(TestConfig cfg)
    {
        // Edge dùng cùng nhân Chromium nên cú pháp tham số giống Chrome
        var options = new EdgeOptions();
        if (cfg.Headless)
        {
            options.AddArgument("--headless=new");
        }
        options.AddArgument($"--window-size={cfg.WindowWidth},{cfg.WindowHeight}");
        options.AddUserProfilePreference("credentials_enable_service", false);
        options.AddUserProfilePreference("profile.password_manager_enabled", false);
        options.AddUserProfilePreference("profile.password_manager_leak_detection", false);
        return options;
    }

    private static FirefoxOptions FirefoxOpts(TestConfig cfg)
    {
        // Firefox khác Chrome: --headless (không có =new) và --width/--height
        var options = new FirefoxOptions();
        if (cfg.Headless)
        {
            options.AddArgument("--headless");
        }
        options.AddArgument($"--width={cfg.WindowWidth}");
        options.AddArgument($"--height={cfg.WindowHeight}");
        options.SetPreference("signon.rememberSignons", false);
        return options;
    }
}
