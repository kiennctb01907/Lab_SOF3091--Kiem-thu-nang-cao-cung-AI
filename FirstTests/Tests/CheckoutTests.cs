using FirstTests.Core;
using FirstTests.Pages;

namespace FirstTests.Tests;

/// <summary>
/// Lab 6 - Phần E: các ca kiểm thử ô Zip/Postal Code do AI đề xuất, ĐÃ LỌC.
/// AI đề xuất 10 ca (TC01-TC10). Giữ 7 ca dùng được, sửa lại TC10, bỏ TC06 và TC07 (lý do ghi trong README.md).
/// Không viết [SetUp] trong file test: đăng nhập và mở trang Checkout bằng hàm MoTrangCheckout().
/// </summary>
[TestFixture]
[Category("Checkout")]
public class CheckoutTests : BaseTest
{
    private const string Ho = "Nguyen";
    private const string Ten = "An";

    /// <summary>Đăng nhập rồi mở trang Checkout: Your Information.</summary>
    private CheckoutPage MoTrangCheckout()
    {
        new LoginPage(driver).DangNhap("standard_user", "secret_sauce");
        return new CheckoutPage(driver).Mo();
    }

    // TC01
    [Test, Category("Smoke")]
    public void MaBuuChinh_HopLe5ChuSo_SangBuocHai()
    {
        CheckoutPage trang = MoTrangCheckout().DienThongTin(Ho, Ten, "70000").BamTiepTuc();

        Assert.That(trang.DaSangBuocHai(), Is.True, "Mã 70000 phải cho sang bước Checkout: Overview");
    }

    // TC02
    [Test]
    public void MaBuuChinh_BoTrong_BaoLoiBatBuoc()
    {
        CheckoutPage trang = MoTrangCheckout().DienThongTin(Ho, Ten, "").BamTiepTuc();

        Assert.Multiple(() =>
        {
            Assert.That(trang.DaSangBuocHai(), Is.False, "Bỏ trống thì phải ở lại bước 1");
            Assert.That(trang.LayThongBaoLoi(), Is.EqualTo("Error: Postal Code is required"));
        });
    }

    // TC03 - biên dưới
    [Test]
    public void MaBuuChinh_MotKyTu_SangBuocHai()
    {
        CheckoutPage trang = MoTrangCheckout().DienThongTin(Ho, Ten, "1").BamTiepTuc();

        Assert.That(trang.DaSangBuocHai(), Is.True, "Trang chỉ bắt buộc không trống, 1 ký tự vẫn hợp lệ");
    }

    // TC04 - biên trên
    [Test]
    public void MaBuuChinh_Chuoi100KyTu_GiuNguyenVaSangBuocHai()
    {
        string maDai = new string('9', 100);
        CheckoutPage trang = MoTrangCheckout().DienThongTin(Ho, Ten, maDai);

        Assert.That(trang.LayMaBuuChinhDangNhap(), Is.EqualTo(maDai), "Ô Postal Code không được cắt bớt");

        trang.BamTiepTuc();

        Assert.That(trang.DaSangBuocHai(), Is.True);
    }

    // TC05
    [Test]
    public void MaBuuChinh_CoChuVaKhoangTrang_SangBuocHai()
    {
        CheckoutPage trang = MoTrangCheckout().DienThongTin(Ho, Ten, "SW1A 1AA").BamTiepTuc();

        Assert.That(trang.DaSangBuocHai(), Is.True, "Mã kiểu Anh có chữ và khoảng trắng vẫn được nhận");
    }

    // TC08
    [Test]
    public void MaBuuChinh_KhoangTrangDauCuoi_SangBuocHai()
    {
        CheckoutPage trang = MoTrangCheckout().DienThongTin(Ho, Ten, "  70000  ").BamTiepTuc();

        Assert.That(trang.DaSangBuocHai(), Is.True);
    }

    // TC09
    [Test]
    public void BaOTrong_BaoLoiFirstNameTruoc()
    {
        CheckoutPage trang = MoTrangCheckout().DienThongTin("", "", "").BamTiepTuc();

        Assert.Multiple(() =>
        {
            Assert.That(trang.DaSangBuocHai(), Is.False);
            Assert.That(trang.LayThongBaoLoi(), Is.EqualTo("Error: First Name is required"),
                "Cả 3 ô trống thì phải báo First Name trước");
        });
    }

    // TC10 - đã sửa: bản của AI gọi DienThongTin lần 2 nên gõ nối thành "NguyenNguyen", "AnAn"
    [Test]
    public void MaBuuChinh_DongLoiRoiNhapLai_SangBuocHai()
    {
        CheckoutPage trang = MoTrangCheckout().DienThongTin(Ho, Ten, "").BamTiepTuc();
        Assert.That(trang.CoThongBaoLoi(), Is.True, "Phải có thông báo lỗi trước khi đóng");

        trang.DongThongBaoLoi();
        Assert.That(trang.CoThongBaoLoi(), Is.False, "Bấm X xong thông báo lỗi phải biến mất");

        trang.NhapLaiMaBuuChinh("70000").BamTiepTuc();

        Assert.That(trang.DaSangBuocHai(), Is.True, "Nhập lại mã hợp lệ thì phải sang bước 2");
    }
}
