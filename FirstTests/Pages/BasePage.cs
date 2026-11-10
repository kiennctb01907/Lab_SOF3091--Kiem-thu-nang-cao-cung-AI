using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace FirstTests.Pages;

/// <summary>
/// Lớp cha của mọi Page Object: giữ driver, WebDriverWait và hàm Tim(By) tìm phần tử KÈM chờ (Explicit Wait).
/// Page con kế thừa lớp này, không tự khai báo lại driver/wait.
/// </summary>
public abstract class BasePage
{
    protected readonly IWebDriver driver;
    protected readonly WebDriverWait wait;

    protected BasePage(IWebDriver driver)
    {
        this.driver = driver;
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    }

    /// <summary>Tìm 1 phần tử, chờ tối đa 10 giây cho đến khi nó xuất hiện trong DOM.</summary>
    protected IWebElement Tim(By locator) => wait.Until(d => d.FindElement(locator));
}
