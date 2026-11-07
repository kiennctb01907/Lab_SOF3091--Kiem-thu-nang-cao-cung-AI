using OpenQA.Selenium;

namespace FirstTests.Lab3;

/// <summary>
/// Lab 3 - Phần A: chứng minh vấn đề đồng bộ trên trang dynamic_loading/2.
/// Phần tử #finish chỉ được JavaScript tạo ra vài giây SAU khi bấm Start.
/// </summary>
[TestFixture]
public class DynamicLoadingTests : Lab3TestBase
{
    private const string Url = "https://the-internet.herokuapp.com/dynamic_loading/2";

    /// <summary>
    /// Bước 1: tìm #finish NGAY sau khi bấm Start, không chờ -> rớt với NoSuchElementException.
    /// Gắn [Explicit] để Run All vẫn xanh; muốn xem lại lỗi thì chạy riêng test này.
    /// </summary>
    [Test, Explicit("Phần A bước 1: cố ý rớt để chứng minh vấn đề đồng bộ")]
    public void Step1_FindFinishWithoutWait_Fails()
    {
        driver.Navigate().GoToUrl(Url);
        driver.FindElement(By.CssSelector("#start button")).Click();

        IWebElement finish = driver.FindElement(By.Id("finish"));

        Assert.That(finish.Text, Is.EqualTo("Hello World!"));
    }

    /// <summary>Bước 2: thêm Explicit Wait (WebDriverWait + Until) -> pass.</summary>
    [Test]
    public void Step2_FindFinishWithExplicitWait_Passes()
    {
        driver.Navigate().GoToUrl(Url);
        driver.FindElement(By.CssSelector("#start button")).Click();

        IWebElement finish = wait.Until(d =>
        {
            IWebElement e = d.FindElement(By.Id("finish"));
            return e.Displayed ? e : null;
        });
        Console.WriteLine($"Nội dung #finish: {finish.Text}");

        Assert.That(finish.Text, Is.EqualTo("Hello World!"));
    }
}
