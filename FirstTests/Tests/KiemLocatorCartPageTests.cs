using FirstTests.Core;
using FirstTests.Pages;
using OpenQA.Selenium;

namespace FirstTests.Tests;

/// <summary>
/// Lab 7 - Phần C, điểm 1 của checklist ("locator thật"): đếm số phần tử khớp với từng locator AI dùng trong CartPage
/// trên trang cart.html thật (giỏ có 2 sản phẩm). Locator nào đếm ra 0 là locator AI bịa.
/// Giữ lại trong project làm bằng chứng thẩm định; chạy riêng bằng Category ThamDinhAI.
/// </summary>
[TestFixture]
[Category("ThamDinhAI")]
public class KiemLocatorCartPageTests : BaseTest
{
    [Test]
    public void LocatorCuaAI_DeuCoTrenTrangGioHang()
    {
        new LoginPage(driver).DangNhap("standard_user", "secret_sauce")
            .ThemVaoGio("Sauce Labs Backpack")
            .ThemVaoGio("Sauce Labs Bike Light");
        new CartPage(driver).MoGioHang();

        var locatorCuaAI = new (string Ten, By Locator, int SoMongDoi)[]
        {
            ("bieuTuongGio  .shopping_cart_link", By.CssSelector(".shopping_cart_link"), 1),
            ("huyHieuGio    .shopping_cart_badge", By.CssSelector(".shopping_cart_badge"), 1),
            ("danhSachGio   .cart_list", By.CssSelector(".cart_list"), 1),
            ("dongSanPham   .cart_item", By.CssSelector(".cart_item"), 2),
            ("tenSanPham    .inventory_item_name", By.CssSelector(".inventory_item_name"), 2),
            ("giaSanPham    .inventory_item_price", By.CssSelector(".inventory_item_price"), 2),
            ("soLuong       .cart_quantity", By.CssSelector(".cart_quantity"), 2),
            ("nutXoa        button.cart_button", By.CssSelector("button.cart_button"), 2),
            ("nutTiepTucMua #continue-shopping", By.Id("continue-shopping"), 1),
            ("nutThanhToan  #checkout", By.Id("checkout"), 1),
        };

        Assert.Multiple(() =>
        {
            foreach (var (ten, locator, soMongDoi) in locatorCuaAI)
            {
                int soThat = driver.FindElements(locator).Count;
                TestContext.Out.WriteLine($"{ten,-40} tìm thấy {soThat} (cần {soMongDoi})");
                Assert.That(soThat, Is.EqualTo(soMongDoi), $"Locator {ten}");
            }
        });
    }
}
