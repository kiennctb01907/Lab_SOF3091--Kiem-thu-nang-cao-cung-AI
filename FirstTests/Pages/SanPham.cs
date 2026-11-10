using System.Globalization;

namespace FirstTests.Pages;

/// <summary>
/// Dữ liệu 1 sản phẩm trên trang inventory (kiểu giá trị thuần, không chứa phần tử web).
/// GiaHienThi giữ nguyên chữ trên màn hình (ví dụ "$29.99"), Gia là số tương ứng.
/// </summary>
public record SanPham(string Ten, string GiaHienThi)
{
    public decimal Gia => decimal.Parse(GiaHienThi.TrimStart('$'), CultureInfo.InvariantCulture);
}
