using System.Text.Json;

namespace FirstTests.Data;

/// <summary>
/// Một bộ dữ liệu đăng nhập trong Data/users.json.
/// Tên thuộc tính phải trùng y hệt tên khóa trong JSON (phân biệt hoa thường).
/// </summary>
public class LoginData
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    /// <summary>Kỳ vọng: true là phải vào được trang sản phẩm, false là phải ở lại trang đăng nhập và báo lỗi.</summary>
    public bool ThanhCong { get; set; }

    public string GhiChu { get; set; } = string.Empty;

    /// <summary>Đọc toàn bộ bộ dữ liệu từ Data/users.json trong thư mục build.</summary>
    public static List<LoginData> DocTuFile()
    {
        string duongDan = Path.Combine(AppContext.BaseDirectory, "Data", "users.json");
        if (!File.Exists(duongDan))
        {
            throw new FileNotFoundException(
                "Không thấy users.json trong thư mục build. Kiểm tra CopyToOutputDirectory trong FirstTests.csproj.", duongDan);
        }

        return JsonSerializer.Deserialize<List<LoginData>>(File.ReadAllText(duongDan))
            ?? throw new InvalidOperationException("users.json rỗng.");
    }
}
