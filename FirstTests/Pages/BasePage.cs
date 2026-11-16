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
