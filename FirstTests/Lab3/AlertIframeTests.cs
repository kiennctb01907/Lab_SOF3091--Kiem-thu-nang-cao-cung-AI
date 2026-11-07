using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace FirstTests.Lab3;

/// <summary>
/// Lab 3 - Phần C: xử lý 3 loại JavaScript alert và gõ chữ vào trình soạn thảo trong iframe.
/// </summary>
[TestFixture]
public class AlertIframeTests : Lab3TestBase
{
    private const string AlertsUrl = "https://the-internet.herokuapp.com/javascript_alerts";

    /// <summary>Chờ alert xuất hiện rồi chuyển vào alert.</summary>
    private IAlert WaitForAlert() => wait.Until(d =>
    {
        try { return d.SwitchTo().Alert(); }
        catch (NoAlertPresentException) { return null; }
    })!;

    private string ResultText() => driver.FindElement(By.Id("result")).Text;

    [Test]
    public void JsAlert_Accept_ShouldShowSuccessResult()
    {
        driver.Navigate().GoToUrl(AlertsUrl);
        driver.FindElement(By.CssSelector("button[onclick='jsAlert()']")).Click();

        IAlert alert = WaitForAlert();
        Assert.That(alert.Text, Is.EqualTo("I am a JS Alert")); // Assert nội dung TRƯỚC khi Accept
        alert.Accept();

        Console.WriteLine($"#result: {ResultText()}");
        Assert.That(ResultText(), Is.EqualTo("You successfully clicked an alert"));
    }

    [Test]
    public void JsConfirm_Dismiss_ShouldShowCancelResult()
    {
        driver.Navigate().GoToUrl(AlertsUrl);
        driver.FindElement(By.CssSelector("button[onclick='jsConfirm()']")).Click();

        IAlert confirm = WaitForAlert();
        Assert.That(confirm.Text, Is.EqualTo("I am a JS Confirm"));
        confirm.Dismiss();

        Console.WriteLine($"#result: {ResultText()}");
        Assert.That(ResultText(), Is.EqualTo("You clicked: Cancel"));
    }

    [Test]
    public void JsPrompt_SendKeysAndAccept_ShouldShowEnteredText()
    {
        const string input = "Xin chao Selenium";
        driver.Navigate().GoToUrl(AlertsUrl);
        driver.FindElement(By.CssSelector("button[onclick='jsPrompt()']")).Click();

        IAlert prompt = WaitForAlert();
        Assert.That(prompt.Text, Is.EqualTo("I am a JS prompt"));
        prompt.SendKeys(input);
        prompt.Accept();

        Console.WriteLine($"#result: {ResultText()}");
        Assert.That(ResultText(), Is.EqualTo($"You entered: {input}"));
    }

    [Test]
    public void Iframe_TypeIntoEditor_ThenBackToDefaultContent()
    {
        const string text = "Nhom em dang test iframe bang Selenium";
        driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/iframe");

        // Vào iframe của trình soạn thảo TinyMCE
        wait.Until(d => d.FindElements(By.Id("mce_0_ifr")).Count > 0);
        driver.SwitchTo().Frame("mce_0_ifr");

        // Chờ TinyMCE khởi tạo xong: thân trình soạn thảo cho phép gõ và đã có nội dung mặc định
        IWebElement editor = wait.Until(d =>
        {
            IWebElement body = d.FindElement(By.Id("tinymce"));
            return body.GetAttribute("contenteditable") == "true" && body.Text.Length > 0 ? body : null;
        })!;

        // Clear() không dùng được với vùng contenteditable nên chọn hết bằng Ctrl+A rồi gõ đè
        editor.Click();
        editor.SendKeys(Keys.Control + "a");
        editor.SendKeys(text);
        string typed = wait.Until(d => d.FindElement(By.Id("tinymce")).Text == text ? text : null)!;
        Console.WriteLine($"Nội dung trong iframe: {typed}");

        // Vào thì phải ra: quay về trang chính
        driver.SwitchTo().DefaultContent();
        string heading = driver.FindElement(By.TagName("h3")).Text;

        Assert.Multiple(() =>
        {
            Assert.That(typed, Is.EqualTo(text), "Chữ gõ vào trình soạn thảo phải hiện đúng");
            Assert.That(heading, Does.Contain("Editor"), "Sau DefaultContent() phải đọc được tiêu đề của trang chính");
        });
    }
}
