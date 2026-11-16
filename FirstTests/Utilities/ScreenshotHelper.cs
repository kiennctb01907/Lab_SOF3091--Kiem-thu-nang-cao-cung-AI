using OpenQA.Selenium;

namespace FirstTests.Utilities;

/// <summary>Chụp màn hình trình duyệt và lưu vào thư mục FailureScreenshots (tách từ TearDown của Bài 3).</summary>
public static class ScreenshotHelper
{
    /// <summary>Chụp ảnh, lưu thành {tenTest}.png và trả về đường dẫn file.</summary>
    public static string Chup(IWebDriver driver, string tenTest)
    {
        string thuMuc = Path.Combine(TestContext.CurrentContext.WorkDirectory, "FailureScreenshots");
        Directory.CreateDirectory(thuMuc);

        // Tên ca data-driven có thể chứa ký tự không hợp lệ cho tên file
        string tenFile = string.Concat(tenTest.Select(c => Path.GetInvalidFileNameChars().Contains(c) ? '_' : c));
        string duongDan = Path.Combine(thuMuc, $"{tenFile}.png");

        ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile(duongDan);
        return duongDan;
    }
}
