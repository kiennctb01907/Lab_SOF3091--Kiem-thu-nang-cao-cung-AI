using FirstTests.Core;

namespace FirstTests.Tests;

/// <summary>
/// Lab 1 - Phần B: kịch bản Selenium đầu tiên, chuyển sang kế thừa BaseTest ở Lab 5.
/// Mở trang chủ Selenium (SeleniumHomeUrl trong appsettings.json) -> đọc tiêu đề -> Assert.
/// </summary>
[TestFixture]
public class FirstSeleniumTest : BaseTest
{
    [Test]
    public void OpenSeleniumHomePage_TitleShouldContainSelenium()
    {
        driver.Navigate().GoToUrl(cfg.SeleniumHomeUrl);

        string title = driver.Title;
        Console.WriteLine($"Tiêu đề trang: {title}");

        Assert.That(title, Does.Contain("Selenium"), "Tiêu đề trang phải chứa chữ 'Selenium'");
        Assert.That(driver.Url, Does.StartWith(cfg.SeleniumHomeUrl.TrimEnd('/')));
    }
}
