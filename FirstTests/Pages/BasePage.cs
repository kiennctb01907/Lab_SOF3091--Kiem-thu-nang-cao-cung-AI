using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace FirstTests.Pages;

/// <summary>
/// Lớp cha của mọi Page Object: giữ driver, WebDriverWait và hàm Tim(By) tìm phần tử KÈM chờ (Explicit Wait).
/// Page con kế thừa lớp này, không tự khai báo lại driver/wait.
/// </summary>
public abstract class BasePage
{
    private static readonly TimeSpan ThoiGianCho = TimeSpan.FromSeconds(10);

    protected readonly IWebDriver driver;
    protected readonly WebDriverWait wait;

    protected BasePage(IWebDriver driver)
    {
        this.driver = driver;
        wait = new WebDriverWait(driver, ThoiGianCho);
    }

    /// <summary>Tìm 1 phần tử, chờ tối đa 10 giây cho đến khi nó xuất hiện trong DOM.</summary>
    protected IWebElement Tim(By locator) => wait.Until(d => d.FindElement(locator));

    /// <summary>Tìm 1 phần tử, chờ tối đa 10 giây cho đến khi nó xuất hiện VÀ đang hiển thị.</summary>
    protected IWebElement TimHienThi(By locator) => wait.Until(d =>
    {
        IWebElement e = d.FindElement(locator);
        return e.Displayed ? e : null;
    })!;

    /// <summary>Tìm danh sách phần tử, chờ cho đến khi có ít nhất 1 phần tử.</summary>
    protected IReadOnlyCollection<IWebElement> TimTatCa(By locator) => wait.Until(d =>
    {
        IReadOnlyCollection<IWebElement> list = d.FindElements(locator);
        return list.Count > 0 ? list : null;
    })!;

    /// <summary>
    /// Tìm 1 phần tử NGAY LẬP TỨC, không chờ. Chỉ dùng để chứng minh vấn đề đồng bộ ở Bài 3
    /// (test cố ý rớt); các hành vi bình thường phải dùng Tim().
    /// </summary>
    protected IWebElement TimKhongCho(By locator) => driver.FindElement(locator);

    /// <summary>Địa chỉ trang hiện tại, để test kiểm tra người dùng đang ở đâu.</summary>
    public string LayUrl() => driver.Url;
}
