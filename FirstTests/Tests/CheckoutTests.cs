using FirstTests.Core;
using FirstTests.Pages;

namespace FirstTests.Tests;

/// <summary>
/// Lab 6 - Phần E: các ca kiểm thử ô Zip/Postal Code do AI đề xuất, ĐÃ LỌC.
/// Chỉ giữ 5 ca đúng với hành vi thật của saucedemo; 6 ca còn lại bị loại (lý do ghi trong README.md).
/// </summary>
[TestFixture]
[Category("Checkout")]
public class CheckoutTests : BaseTest
{
    /// <summary>Đăng nhập rồi mở trang Checkout: Your Information.</summary>
    private CheckoutPage MoTrangCheckout()
    {
        new LoginPage(driver).DangNhap("standard_user", "secret_sauce");
        return new CheckoutPage(driver).Mo();
    }

    [Test, Category("Smoke")]
    public void MaBuuChinh_HopLe5ChuSo_SangBuocHai()
    {
        CheckoutPage trang = MoTrangCheckout().DienThongTin("Nguyen", "An", "70000").BamTiepTuc();

        Assert.That(trang.DaSangBuocHai(), Is.True, "Mã 70000 phải cho sang bước Checkout: Overview");
    }

    [Test]
    public void MaBuuChinh_BoTrong_BaoLoiBatBuoc()
    {
        CheckoutPage trang = MoTrangCheckout().DienThongTin("Nguyen", "An", "").BamTiepTuc();

        Assert.Multiple(() =>
        {
            Assert.That(trang.LayThongBaoLoi(), Is.EqualTo("Error: Postal Code is required"));
            Assert.That(trang.DaSangBuocHai(), Is.False, "Bỏ trống thì phải ở lại bước 1");
        });
    }

    [Test]
    public void MaBuuChinh_DinhDangZipCong4_SangBuocHai()
    {
        CheckoutPage trang = MoTrangCheckout().DienThongTin("Nguyen", "An", "12345-6789").BamTiepTuc();

        Assert.That(trang.DaSangBuocHai(), Is.True);
    }

    [Test]
    public void MaBuuChinh_CoMaNhungThieuFirstName_BaoLoiFirstName()
    {
        CheckoutPage trang = MoTrangCheckout().DienThongTin("", "An", "70000").BamTiepTuc();

        Assert.That(trang.LayThongBaoLoi(), Is.EqualTo("Error: First Name is required"));
    }

    [Test]
    public void MaBuuChinh_DongThongBaoLoi_ThongBaoBienMat()
    {
        CheckoutPage trang = MoTrangCheckout().DienThongTin("Nguyen", "An", "").BamTiepTuc();
        Assert.That(trang.CoThongBaoLoi(), Is.True, "Phải có thông báo lỗi trước khi đóng");

        trang.DongThongBaoLoi();

        Assert.That(trang.CoThongBaoLoi(), Is.False, "Bấm X xong thông báo lỗi phải biến mất");
    }
}
