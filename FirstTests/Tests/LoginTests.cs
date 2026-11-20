using System.Text.RegularExpressions;
using FirstTests.Core;
using FirstTests.Data;
using FirstTests.Pages;

namespace FirstTests.Tests;

/// <summary>
/// Đăng nhập saucedemo. BaseTest đã mở sẵn trang đăng nhập (BaseUrl) nên test không tự mở trang.
/// Lab 5 - Phần D: DangNhap_TheoDuLieu sinh ra 1 ca test cho mỗi bộ trong Data/users.json.
/// </summary>
[TestFixture]
[Category("Login")]
public class LoginTests : BaseTest
{
    // ---------------- Lab 5 - Phần D: Data-Driven ----------------

    /// <summary>
    /// Nguồn dữ liệu phải static vì NUnit gọi hàm này trước khi tạo đối tượng LoginTests.
    /// Thêm một bộ vào users.json là có thêm một ca test, không phải sửa file .cs.
    /// </summary>
    public static IEnumerable<TestCaseData> DuLieuDangNhap()
    {
        List<LoginData> danhSach = LoginData.DocTuFile();
        for (int i = 0; i < danhSach.Count; i++)
        {
            LoginData u = danhSach[i];
            // Có số thứ tự để tên không trùng khi 2 bộ cùng Username (ví dụ cùng để trống)
            yield return new TestCaseData(u)
                .SetName($"DangNhap_{i + 1:00}_{TenGon(u.Username)}{(u.Password.Length == 0 ? "_MatKhauTrong" : "")}")
                .SetDescription(u.GhiChu);
        }
    }

    [TestCaseSource(nameof(DuLieuDangNhap))]
    public void DangNhap_TheoDuLieu(LoginData u)
    {
        var trangDangNhap = new LoginPage(driver);
        InventoryPage trangSanPham = trangDangNhap.DangNhap(u.Username, u.Password);

        bool vaoDuocTrangSanPham = trangSanPham.DaHienThi();
        Console.WriteLine($"Ghi chú: {u.GhiChu}");
        Console.WriteLine($"Kỳ vọng ThanhCong = {u.ThanhCong}, thực tế vào được trang sản phẩm = {vaoDuocTrangSanPham}");

        if (u.ThanhCong)
        {
            Assert.Multiple(() =>
            {
                Assert.That(vaoDuocTrangSanPham, Is.True, $"Phải vào được trang sản phẩm. {u.GhiChu}");
                Assert.That(trangSanPham.LayUrl(), Is.EqualTo(trangSanPham.Url));
            });
        }
        else
        {
            Console.WriteLine($"Thông báo lỗi: {(trangDangNhap.ThongBaoLoiDaHienThi() ? trangDangNhap.LayThongBaoLoi() : "(không có)")}");
            Assert.Multiple(() =>
            {
                Assert.That(vaoDuocTrangSanPham, Is.False, $"Không được vào trang sản phẩm. {u.GhiChu}");
                Assert.That(trangDangNhap.ThongBaoLoiDaHienThi(), Is.True, "Phải hiện thông báo lỗi");
                Assert.That(trangDangNhap.LayUrl(), Is.EqualTo(trangDangNhap.Url), "Phải ở lại trang đăng nhập");
            });
        }
    }

    /// <summary>Rút gọn Username thành chữ dùng được trong tên ca test (bỏ ký tự lạ, cắt ngắn, trống thì ghi "Trong").</summary>
    private static string TenGon(string username)
    {
        string ten = Regex.Replace(username.Trim(), @"[^A-Za-z0-9_]", "");
        if (ten.Length == 0) return username.Length == 0 ? "Trong" : "KyTuDacBiet";
        if (ten.Length > 20) ten = ten[..20] + $"_Dai{username.Length}";
        if (username != username.Trim()) ten += "_CoKhoangTrang";
        return ten;
    }

    // ---------------- Lab 2 - Phần B (giữ lại) ----------------

    [Test, Category("Smoke")]
    public void Login_StandardUser_ShouldGoToInventoryPage()
    {
        InventoryPage trangSanPham = new LoginPage(driver).DangNhap("standard_user", "secret_sauce");

        Assert.That(trangSanPham.DaHienThi(), Is.True, "Danh sách sản phẩm phải hiện ra");
        Console.WriteLine($"URL sau khi đăng nhập: {trangSanPham.LayUrl()}");

        Assert.That(trangSanPham.LayUrl(), Is.EqualTo(trangSanPham.Url),
            "Đăng nhập đúng phải chuyển sang trang inventory.html");
    }

    [Test]
    public void Login_LockedOutUser_ShouldShowErrorMessage()
    {
        var trangDangNhap = new LoginPage(driver);
        trangDangNhap.DangNhap("locked_out_user", "secret_sauce");

        string thongBao = trangDangNhap.LayThongBaoLoi();
        Console.WriteLine($"Thông báo lỗi: {thongBao}");

        Assert.Multiple(() =>
        {
            Assert.That(trangDangNhap.ThongBaoLoiDaHienThi(), Is.True, "Thông báo lỗi phải hiện ra");
            Assert.That(thongBao, Is.EqualTo("Epic sadface: Sorry, this user has been locked out."),
                "Nội dung thông báo lỗi không đúng");
            Assert.That(trangDangNhap.LayUrl(), Is.EqualTo(trangDangNhap.Url), "Tài khoản bị khóa thì phải ở lại trang đăng nhập");
        });
    }

    // ---------------- Lab 2 - Phần C (giữ lại) ----------------

    [Test, Category("Smoke")]
    public void Inventory_ShouldListSixProductsWithValidNameAndPrice()
    {
        InventoryPage trangSanPham = new LoginPage(driver).DangNhap("standard_user", "secret_sauce");

        List<string> names = trangSanPham.LayDanhSachTenSanPham();
        List<string> prices = trangSanPham.LayDanhSachGiaHienThi();
        List<decimal> giaSo = trangSanPham.LayDanhSachGia();

        for (int i = 0; i < names.Count; i++)
        {
            Console.WriteLine($"{names[i]} - {prices[i]}");
        }

        Assert.Multiple(() =>
        {
            Assert.That(names, Has.Count.EqualTo(6), "Trang inventory phải có đúng 6 sản phẩm");
            Assert.That(prices, Has.Count.EqualTo(names.Count), "Mỗi sản phẩm phải có đúng một giá");
            Assert.That(names, Has.None.Empty, "Không được có tên sản phẩm rỗng");
            Assert.That(names, Is.Unique, "Tên sản phẩm không được trùng nhau");
            Assert.That(names, Does.Contain("Sauce Labs Backpack"), "Danh sách phải có sản phẩm Sauce Labs Backpack");
            Assert.That(prices, Has.All.StartWith("$"), "Mọi giá đều phải bắt đầu bằng ký tự $");
            Assert.That(giaSo, Has.All.GreaterThan(0m), "Giá sản phẩm phải là số dương");
            Assert.That(prices[names.IndexOf("Sauce Labs Backpack")], Is.EqualTo("$29.99"),
                "Giá của Sauce Labs Backpack phải là $29.99");
        });
    }
}
