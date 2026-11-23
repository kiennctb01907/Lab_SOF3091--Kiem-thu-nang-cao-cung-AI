using System.Diagnostics;
using FirstTests.Config;
using FirstTests.Utilities;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;

namespace FirstTests.Core;

/// <summary>
/// Lớp cha của mọi file test, gom vòng đời trình duyệt về một chỗ:
/// SetUp: ghi log bắt đầu -> đọc config -> tạo driver qua DriverFactory -> mở BaseUrl.
/// TearDown: ghi log OK/RỚT, chụp ảnh nếu test Failed, sau đó LUÔN đóng trình duyệt.
/// driver, cfg, đồng hồ đều là field thường của từng đối tượng test (không static),
/// nên khi chạy song song mỗi fixture giữ trình duyệt riêng của nó.
/// </summary>
public abstract class BaseTest
{
    protected IWebDriver driver = null!;
    protected TestConfig cfg = null!;
    private readonly Stopwatch dongHo = new();

    [SetUp]
    public void KhoiTaoTrinhDuyet()
    {
        dongHo.Restart();
        cfg = ConfigReader.Get();
        Log.Info($"BẮT ĐẦU  {TenCa()} | trình duyệt {cfg.Browser}{(cfg.Headless ? " (headless)" : "")}");
        driver = DriverFactory.Create(cfg);
        driver.Navigate().GoToUrl(cfg.BaseUrl);
    }

    [TearDown]
    public void DongTrinhDuyet()
    {
        var ketQua = TestContext.CurrentContext.Result;
        string thoiGian = $"{dongHo.Elapsed.TotalSeconds:0.0}s";
        try
        {
            if (ketQua.Outcome.Status == TestStatus.Failed)
            {
                Log.Error($"RỚT     {TenCa()} ({thoiGian}) | {DongDau(ketQua.Message)}");
                if (driver is not null)
                {
                    string duongDan = ScreenshotHelper.Chup(driver, TestContext.CurrentContext.Test.Name);
                    TestContext.AddTestAttachment(duongDan, "Ảnh chụp màn hình lúc test thất bại");
                    Log.Info($"Đã chụp ảnh khi test rớt: {duongDan}");
                }
            }
            else
            {
                Log.Info($"OK       {TenCa()} ({thoiGian})");
            }
        }
        catch (Exception ex)
        {
            // Chụp ảnh hỏng (trình duyệt đã treo, hết bộ nhớ...) cũng không được làm test báo sai lý do
            Log.Warn($"Không chụp được ảnh: {ex.Message}");
        }
        finally
        {
            // Đặt trong finally để chụp ảnh lỗi cũng không để lại trình duyệt chạy ngầm
            driver?.Quit();
            driver?.Dispose();
            // Gán lại null: nếu SetUp của ca sau hỏng trước khi tạo driver mới thì TearDown không đụng vào driver cũ đã đóng
            driver = null!;
        }
    }

    private static string TenCa() => $"{TestContext.CurrentContext.Test.ClassName?.Split('.').Last()}.{TestContext.CurrentContext.Test.Name}";

    /// <summary>Lấy dòng đầu của thông báo lỗi để nhật ký gọn, chi tiết đầy đủ vẫn có trong Test Explorer.</summary>
    private static string DongDau(string? thongBao) =>
        (thongBao ?? string.Empty).Split('\n').Select(d => d.Trim()).FirstOrDefault(d => d.Length > 0) ?? "(không có thông báo)";
}
