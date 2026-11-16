using FirstTests.Config;
using FirstTests.Utilities;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;

namespace FirstTests.Core;

/// <summary>
/// Lớp cha của mọi file test, gom vòng đời trình duyệt về một chỗ:
/// SetUp: đọc config -> tạo driver qua DriverFactory -> mở BaseUrl.
/// TearDown: chụp ảnh nếu test Failed, sau đó LUÔN đóng trình duyệt.
/// Chỉ dùng Explicit Wait (nằm trong Page), không bật Implicit Wait.
/// </summary>
public abstract class BaseTest
{
    protected IWebDriver driver = null!;
    protected TestConfig cfg = null!;

    [SetUp]
    public void KhoiTaoTrinhDuyet()
    {
        cfg = ConfigReader.Get();
        driver = DriverFactory.Create(cfg);
        driver.Navigate().GoToUrl(cfg.BaseUrl);
    }

    [TearDown]
    public void DongTrinhDuyet()
    {
        try
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed && driver != null)
            {
                string duongDan = ScreenshotHelper.Chup(driver, TestContext.CurrentContext.Test.Name);
                TestContext.AddTestAttachment(duongDan, "Ảnh chụp màn hình lúc test thất bại");
                Console.WriteLine($"Đã chụp ảnh khi test rớt: {duongDan}");
            }
        }
        catch (Exception ex)
        {
            // Chụp ảnh hỏng (trình duyệt đã treo, hết bộ nhớ...) cũng không được làm test báo sai lý do
            Console.WriteLine($"Không chụp được ảnh: {ex.Message}");
        }
        finally
        {
            // Đặt trong finally để chụp ảnh lỗi cũng không để lại trình duyệt chạy ngầm
            driver?.Quit();
            driver?.Dispose();
        }
    }
}
