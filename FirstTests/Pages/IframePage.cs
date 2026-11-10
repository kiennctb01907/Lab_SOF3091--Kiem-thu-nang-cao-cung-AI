using OpenQA.Selenium;

namespace FirstTests.Pages;

/// <summary>Trang https://the-internet.herokuapp.com/iframe — trình soạn thảo TinyMCE nằm trong iframe.</summary>
public class IframePage : BasePage
{
    public const string Url = "https://the-internet.herokuapp.com/iframe";

    private readonly By khungTrinhSoan = By.Id("mce_0_ifr");
    private readonly By thanTrinhSoan = By.Id("tinymce");
    private readonly By tieuDeTrang = By.TagName("h3");

    public IframePage(IWebDriver driver) : base(driver) { }

    /// <summary>Người dùng mở trang.</summary>
    public IframePage Mo()
    {
        driver.Navigate().GoToUrl(Url);
        return this;
    }

    /// <summary>Người dùng xóa nội dung cũ rồi gõ chữ mới vào trình soạn thảo, xong quay về trang chính.</summary>
    public IframePage GoChuVaoTrinhSoan(string noiDung)
    {
        VaoTrinhSoan();

        // Clear() không dùng được với vùng contenteditable nên chọn hết bằng Ctrl+A rồi gõ đè
        IWebElement than = Tim(thanTrinhSoan);
        than.Click();
        than.SendKeys(Keys.Control + "a");
        than.SendKeys(noiDung);
        wait.Until(d => d.FindElement(thanTrinhSoan).Text == noiDung);

        // Vào thì phải ra: quay về trang chính
        driver.SwitchTo().DefaultContent();
        return this;
    }

    /// <summary>Người dùng đọc chữ đang có trong trình soạn thảo, xong quay về trang chính.</summary>
    public string LayNoiDungTrinhSoan()
    {
        VaoTrinhSoan();
        string noiDung = Tim(thanTrinhSoan).Text;
        driver.SwitchTo().DefaultContent();
        return noiDung;
    }

    /// <summary>Người dùng đọc tiêu đề của trang chính (nằm ngoài iframe).</summary>
    public string LayTieuDeTrang() => Tim(tieuDeTrang).Text;

    /// <summary>Vào iframe và chờ TinyMCE khởi tạo xong: thân cho phép gõ và đã có nội dung mặc định.</summary>
    private void VaoTrinhSoan()
    {
        driver.SwitchTo().Frame(Tim(khungTrinhSoan));
        wait.Until(d =>
        {
            IWebElement than = d.FindElement(thanTrinhSoan);
            return than.GetAttribute("contenteditable") == "true" && than.Text.Length > 0;
        });
    }
}
