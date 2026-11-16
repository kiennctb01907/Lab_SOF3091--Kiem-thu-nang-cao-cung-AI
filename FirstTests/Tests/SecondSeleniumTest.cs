using FirstTests.Core;
using OpenQA.Selenium;

namespace FirstTests.Tests;

/// <summary>
/// Lab 1 - Phần B (mở rộng): kịch bản thứ hai trên example.com, chuyển sang kế thừa BaseTest ở Lab 5.
/// </summary>
[TestFixture]
public class SecondSeleniumTest : BaseTest
{
    [Test]
    public void OpenExampleDomain_TitleAndContentShouldMatch()
    {
        driver.Navigate().GoToUrl(cfg.ExampleUrl);

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
}
