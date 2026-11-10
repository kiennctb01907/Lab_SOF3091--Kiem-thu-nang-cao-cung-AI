using FirstTests.Pages;

namespace FirstTests.Tests;

/// <summary>
/// Lab 3 - Phần C, viết lại bằng Page Object Model: xử lý 3 loại JavaScript alert và gõ chữ vào trình soạn thảo trong iframe.
/// </summary>
[TestFixture]
public class AlertIframeTests : TestBase
{
    [Test]
    public void JsAlert_Accept_ShouldShowSuccessResult()
    {
        JsAlertsPage trang = new JsAlertsPage(driver).Mo().BamJsAlert();

        Assert.That(trang.LayNoiDungHopThoai(), Is.EqualTo("I am a JS Alert")); // Assert nội dung TRƯỚC khi bấm OK
        trang.ChapNhanHopThoai();

        Console.WriteLine($"#result: {trang.LayKetQua()}");
        Assert.That(trang.LayKetQua(), Is.EqualTo("You successfully clicked an alert"));
    }

    [Test]
    public void JsConfirm_Dismiss_ShouldShowCancelResult()
    {
        JsAlertsPage trang = new JsAlertsPage(driver).Mo().BamJsConfirm();

        Assert.That(trang.LayNoiDungHopThoai(), Is.EqualTo("I am a JS Confirm"));
        trang.HuyHopThoai();

        Console.WriteLine($"#result: {trang.LayKetQua()}");
        Assert.That(trang.LayKetQua(), Is.EqualTo("You clicked: Cancel"));
    }

    [Test]
    public void JsPrompt_SendKeysAndAccept_ShouldShowEnteredText()
    {
        const string input = "Xin chao Selenium";
        JsAlertsPage trang = new JsAlertsPage(driver).Mo().BamJsPrompt();

        Assert.That(trang.LayNoiDungHopThoai(), Is.EqualTo("I am a JS prompt"));
        trang.NhapVaoHopThoai(input).ChapNhanHopThoai();

        Console.WriteLine($"#result: {trang.LayKetQua()}");
        Assert.That(trang.LayKetQua(), Is.EqualTo($"You entered: {input}"));
    }

    [Test]
    public void Iframe_TypeIntoEditor_ThenBackToDefaultContent()
    {
        const string text = "Nhom em dang test iframe bang Selenium";
        IframePage trang = new IframePage(driver).Mo().GoChuVaoTrinhSoan(text);

        string typed = trang.LayNoiDungTrinhSoan();
        Console.WriteLine($"Nội dung trong iframe: {typed}");

        Assert.Multiple(() =>
        {
            Assert.That(typed, Is.EqualTo(text), "Chữ gõ vào trình soạn thảo phải hiện đúng");
            Assert.That(trang.LayTieuDeTrang(), Does.Contain("Editor"), "Sau khi thoát iframe phải đọc được tiêu đề của trang chính");
        });
    }
}
