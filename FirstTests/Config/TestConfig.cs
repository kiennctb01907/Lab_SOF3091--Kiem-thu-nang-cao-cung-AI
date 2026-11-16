namespace FirstTests.Config;

/// <summary>
/// Ánh xạ 1-1 với Config/appsettings.json. Tên thuộc tính phải trùng tên khóa trong JSON,
/// lệch một chữ là thuộc tính giữ giá trị mặc định mà không báo lỗi.
/// </summary>
public class TestConfig
{
    /// <summary>Trang chính của bộ test (saucedemo), BaseTest mở trang này trước mỗi test.</summary>
    public string BaseUrl { get; set; } = string.Empty;

    /// <summary>Chrome, Firefox hoặc Edge.</summary>
    public string Browser { get; set; } = string.Empty;

    /// <summary>Thời gian chờ tối đa khi tìm phần tử (Explicit Wait).</summary>
    public int TimeoutSeconds { get; set; }

    /// <summary>Thời gian chờ ngắn cho các hàm hỏi CÓ/KHÔNG (ví dụ DaHienThi), vì ca nghịch phải chờ hết thời gian này.</summary>
    public int CheckTimeoutSeconds { get; set; }

    public bool Headless { get; set; }
    public int WindowWidth { get; set; }
    public int WindowHeight { get; set; }

    /// <summary>Các trang phụ dùng ở Bài 1 và Bài 3.</summary>
    public string TheInternetUrl { get; set; } = string.Empty;
    public string SeleniumHomeUrl { get; set; } = string.Empty;
    public string ExampleUrl { get; set; } = string.Empty;
}
