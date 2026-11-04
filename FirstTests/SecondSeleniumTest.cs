using OpenQA.Selenium;

namespace FirstTests;

/// <summary>
/// Lab 1 - Phần B (mở rộng): Kịch bản Selenium thứ hai trên trang example.com.
/// Mở Chrome -> điều hướng -> đọc và in tiêu đề + đoạn văn đầu tiên -> Assert -> Quit().
/// </summary>
[TestFixture]
public class SecondSeleniumTest
{
    private IWebDriver driver = null!;

    [SetUp]
    public void SetUp()
    {
        driver = DriverFactory.CreateChromeDriver();
    }

    [Test]
    public void OpenExampleDomain_TitleAndContentShouldMatch()
    {
        driver.Navigate().GoToUrl("https://example.com/");

        string title = driver.Title;
        // Lưu ý: example.com hiện không còn thẻ <h1>, nội dung nằm trong thẻ <p> đầu tiên.
        string firstParagraph = driver.FindElement(By.TagName("p")).Text;
        Console.WriteLine($"Tiêu đề trang: {title}");
        Console.WriteLine($"Đoạn văn đầu tiên: {firstParagraph}");

        Assert.Multiple(() =>
        {
            Assert.That(title, Is.EqualTo("Example Domain"), "Tiêu đề trang phải đúng là 'Example Domain'");
            Assert.That(firstParagraph, Does.Contain("documentation examples"),
                "Đoạn văn đầu tiên phải giới thiệu domain dùng cho ví dụ trong tài liệu");
        });
    }

    [TearDown]
    public void TearDown()
    {
        driver?.Quit();
        driver?.Dispose();
    }
}
