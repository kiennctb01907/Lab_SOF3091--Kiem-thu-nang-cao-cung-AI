using FirstTests.Pages;

namespace FirstTests.Tests;

/// <summary>
/// Lab 2 trên https://www.saucedemo.com, viết lại bằng Page Object Model (Lab 4).
/// Phần B: đăng nhập luồng thuận và luồng nghịch. Phần C: thu thập tên và giá 6 sản phẩm.
/// </summary>
[TestFixture]
public class LoginTests : TestBase
{
    private LoginPage trangDangNhap = null!;

    [SetUp]
    public void MoTrangDangNhap()
    {
        trangDangNhap = new LoginPage(driver).Mo();
    }

    // ---------------- Phần B ----------------

    [Test]
    public void Login_StandardUser_ShouldGoToInventoryPage()
    {
        InventoryPage trangSanPham = trangDangNhap.DangNhap("standard_user", "secret_sauce");

        // Chờ danh sách sản phẩm hiện ra rồi mới kiểm tra URL
        Assert.That(trangSanPham.DaHienThi(), Is.True, "Danh sách sản phẩm phải hiện ra");
        Console.WriteLine($"URL sau khi đăng nhập: {trangSanPham.LayUrl()}");

        Assert.That(trangSanPham.LayUrl(), Is.EqualTo(InventoryPage.Url),
            "Đăng nhập đúng phải chuyển sang trang inventory.html");
    }

    [Test]
    public void Login_LockedOutUser_ShouldShowErrorMessage()
    {
        trangDangNhap.DangNhap("locked_out_user", "secret_sauce");

        string thongBao = trangDangNhap.LayThongBaoLoi();
        Console.WriteLine($"Thông báo lỗi: {thongBao}");

        Assert.Multiple(() =>
        {
            Assert.That(trangDangNhap.ThongBaoLoiDaHienThi(), Is.True, "Thông báo lỗi phải hiện ra");
            Assert.That(thongBao, Is.EqualTo("Epic sadface: Sorry, this user has been locked out."),
                "Nội dung thông báo lỗi không đúng");
            Assert.That(trangDangNhap.LayUrl(), Is.EqualTo(LoginPage.Url), "Tài khoản bị khóa thì phải ở lại trang đăng nhập");
        });
    }

    // ---------------- Phần C ----------------

    [Test]
    public void Inventory_ShouldListSixProductsWithValidNameAndPrice()
    {
        InventoryPage trangSanPham = trangDangNhap.DangNhap("standard_user", "secret_sauce");

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
