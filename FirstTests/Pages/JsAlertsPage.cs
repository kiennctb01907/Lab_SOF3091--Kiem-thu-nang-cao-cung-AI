using OpenQA.Selenium;

namespace FirstTests.Pages;

/// <summary>Trang https://the-internet.herokuapp.com/javascript_alerts — 3 loại hộp thoại JavaScript.</summary>
public class JsAlertsPage : BasePage
{
    public const string Url = "https://the-internet.herokuapp.com/javascript_alerts";

    private readonly By nutJsAlert = By.CssSelector("button[onclick='jsAlert()']");
    private readonly By nutJsConfirm = By.CssSelector("button[onclick='jsConfirm()']");
    private readonly By nutJsPrompt = By.CssSelector("button[onclick='jsPrompt()']");
    private readonly By ketQua = By.Id("result");

    public JsAlertsPage(IWebDriver driver) : base(driver) { }

    /// <summary>Người dùng mở trang.</summary>
    public JsAlertsPage Mo()
    {
        driver.Navigate().GoToUrl(Url);
        return this;
    }

    /// <summary>Người dùng bấm nút "Click for JS Alert".</summary>
    public JsAlertsPage BamJsAlert()
    {
        Tim(nutJsAlert).Click();
        return this;
    }

    /// <summary>Người dùng bấm nút "Click for JS Confirm".</summary>
    public JsAlertsPage BamJsConfirm()
    {
        Tim(nutJsConfirm).Click();
        return this;
    }

    /// <summary>Người dùng bấm nút "Click for JS Prompt".</summary>
    public JsAlertsPage BamJsPrompt()
    {
        Tim(nutJsPrompt).Click();
        return this;
    }

    /// <summary>Người dùng đọc nội dung hộp thoại đang hiện (chờ hộp thoại xuất hiện), chưa đóng nó.</summary>
    public string LayNoiDungHopThoai() => ChoHopThoai().Text ?? string.Empty;

    /// <summary>Người dùng bấm OK.</summary>
    public JsAlertsPage ChapNhanHopThoai()
    {
        ChoHopThoai().Accept();
        return this;
    }

    /// <summary>Người dùng bấm Cancel.</summary>
    public JsAlertsPage HuyHopThoai()
    {
        ChoHopThoai().Dismiss();
        return this;
    }

    /// <summary>Người dùng gõ chữ vào ô nhập của hộp thoại prompt.</summary>
    public JsAlertsPage NhapVaoHopThoai(string noiDung)
    {
        ChoHopThoai().SendKeys(noiDung);
        return this;
    }

    /// <summary>Người dùng đọc dòng kết quả hiện dưới các nút.</summary>
    public string LayKetQua() => Tim(ketQua).Text;

    /// <summary>Chờ hộp thoại xuất hiện rồi chuyển vào hộp thoại đó.</summary>
    private IAlert ChoHopThoai() => wait.Until(d =>
    {
        try { return d.SwitchTo().Alert(); }
        catch (NoAlertPresentException) { return null; }
    })!;
}
