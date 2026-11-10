using FirstTests.Pages;

namespace FirstTests.Tests;

/// <summary>
/// Lab 3 - Phần D: test CỐ Ý rớt để kiểm tra cơ chế chụp ảnh trong [TearDown] của TestBase.
/// Gắn [Explicit] để Run All vẫn xanh; chạy riêng test này sẽ sinh ảnh FailureScreenshots/{tên test}.png.
/// </summary>
[TestFixture]
public class FailureScreenshotTests : TestBase
{
    [Test, Explicit("Phần D: cố ý rớt để sinh ảnh chụp khi Failed")]
    public void InventoryFirstPrice_DeliberatelyWrongExpectation_Fails()
    {
        InventoryPage trangSanPham = new LoginPage(driver).Mo().DangNhap("standard_user", "secret_sauce");

        string giaDauTien = trangSanPham.LayGiaSanPhamDauTien();

        // Cố ý sai: giá đầu tiên của trang mặc định là $29.99, không phải $7.99
        Assert.That(giaDauTien, Is.EqualTo("$7.99"), "Cố ý sai để test rớt và sinh ảnh chụp màn hình");
    }
}
