using OpenQA.Selenium;

namespace FirstTests.Pages;

/// <summary>Trang https://the-internet.herokuapp.com/dynamic_loading/2 — nội dung chỉ xuất hiện vài giây SAU khi bấm Start.</summary>
public class DynamicLoadingPage : BasePage
{
    public const string Url = "https://the-internet.herokuapp.com/dynamic_loading/2";

    private readonly By nutStart = By.CssSelector("#start button");
    private readonly By loiChao = By.Id("finish");

    public DynamicLoadingPage(IWebDriver driver) : base(driver) { }

    /// <summary>Người dùng mở trang.</summary>
    public DynamicLoadingPage Mo()
    {
        driver.Navigate().GoToUrl(Url);
        return this;
    }

    /// <summary>Người dùng bấm nút Start.</summary>
    public DynamicLoadingPage BamStart()
    {
        Tim(nutStart).Click();
        return this;
    }

    /// <summary>Người dùng chờ lời chào hiện ra rồi đọc nội dung (có Explicit Wait).</summary>
    public string LayLoiChaoSauKhiChoHienRa() => TimHienThi(loiChao).Text;

    /// <summary>Người dùng đọc lời chào NGAY lập tức, không chờ — cố ý để chứng minh vấn đề đồng bộ.</summary>
    public string LayLoiChaoNgayKhongCho() => TimKhongCho(loiChao).Text;
}
