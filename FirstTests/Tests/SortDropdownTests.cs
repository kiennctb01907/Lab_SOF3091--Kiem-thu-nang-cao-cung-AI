using FirstTests.Pages;

namespace FirstTests.Tests;

/// <summary>
/// Lab 3 - Phần B, viết lại bằng Page Object Model (Lab 4 - Phần D):
/// chọn sắp xếp "Price (low to high)" và Assert danh sách giá THỰC SỰ tăng dần.
/// </summary>
[TestFixture]
public class SortDropdownTests : TestBase
{
    [Test]
    public void SortPriceLowToHigh_PricesShouldBeAscending()
    {
        var trangDangNhap = new LoginPage(driver).Mo();

        // Nối chuỗi lời gọi: đăng nhập -> sắp xếp giá thấp đến cao
        InventoryPage trangSanPham = trangDangNhap.DangNhap("standard_user", "secret_sauce").SapXep("lohi");
        List<decimal> prices = trangSanPham.LayDanhSachGia();
        Console.WriteLine("Giá sau khi sắp xếp: " + string.Join(", ", prices));

        Assert.Multiple(() =>
        {
            Assert.That(trangSanPham.LayKieuSapXepDangChon(), Is.EqualTo("Price (low to high)"), "Dropdown phải đang chọn Price (low to high)");
            Assert.That(prices, Has.Count.EqualTo(6), "Phải có 6 giá");
            Assert.That(prices, Is.EqualTo(prices.Order().ToList()), "Danh sách giá phải tăng dần");
            Assert.That(prices, Is.Ordered.Ascending, "Danh sách giá phải tăng dần");
            Assert.That(prices.First(), Is.EqualTo(prices.Min()), "Giá đầu tiên phải là giá nhỏ nhất");
            Assert.That(prices.Last(), Is.EqualTo(prices.Max()), "Giá cuối cùng phải là giá lớn nhất");
        });
    }
}
