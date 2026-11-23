using FirstTests.Core;
using FirstTests.Pages;

namespace FirstTests.Tests;

/// <summary>
/// Lab 7: các ca kiểm thử giỏ hàng và thanh toán do AI đề xuất ở Phần A, ĐÃ THẨM ĐỊNH ở Phần B.
/// Giữ TC01, 02, 03, 04, 06, 08, 09, 13, 16. Gộp TC05, TC07, TC10 thành một ca tính tổng tiền (sửa lại),
/// bỏ TC11, TC12, TC14 (trùng CheckoutTests của Lab 6) và TC15 (saucedemo không chặn giỏ trống).
/// Không viết [SetUp]: trình duyệt do BaseTest mở, đăng nhập bằng hàm DangNhap().
/// </summary>
[TestFixture]
[Category("GioHang")]
public class GioHangTests : BaseTest
{
    private const string BaLo = "Sauce Labs Backpack";
    private const string DenXe = "Sauce Labs Bike Light";
    private const string AoTreEm = "Sauce Labs Onesie";

    private InventoryPage DangNhap() => new LoginPage(driver).DangNhap("standard_user", "secret_sauce");

    // TC01
    [Test, Category("Smoke")]
    public void ThemMotSanPham_HuyHieuLa1_NutDoiThanhRemove()
    {
        InventoryPage trangSanPham = DangNhap().ThemVaoGio(BaLo);

        Assert.Multiple(() =>
        {
            Assert.That(new CartPage(driver).LaySoTrenHuyHieu(), Is.EqualTo(1));
            Assert.That(trangSanPham.LayChuNutCuaSanPham(BaLo), Is.EqualTo("Remove"));
        });
    }

    // TC02
    [Test]
    public void XemGio_HaiSanPham_DungTenGiaVaSoLuong()
    {
        DangNhap().ThemVaoGio(BaLo).ThemVaoGio(DenXe);
        CartPage gio = new CartPage(driver).MoGioHang();

        Assert.Multiple(() =>
        {
            Assert.That(gio.LayDanhSachTenSanPham(), Is.EqualTo(new[] { BaLo, DenXe }));
            Assert.That(gio.LayGia(BaLo), Is.EqualTo(29.99m));
            Assert.That(gio.LayGia(DenXe), Is.EqualTo(9.99m));
            Assert.That(gio.LaySoLuong(BaLo), Is.EqualTo(1));
            Assert.That(gio.LaySoLuong(DenXe), Is.EqualTo(1));
        });
    }

    // TC03
    [Test]
    public void XoaMotSanPhamTrongGio_ConLaiSanPhamKia()
    {
        DangNhap().ThemVaoGio(BaLo).ThemVaoGio(DenXe);
        CartPage gio = new CartPage(driver).MoGioHang().XoaSanPham(DenXe);

        Assert.Multiple(() =>
        {
            Assert.That(gio.LayDanhSachTenSanPham(), Is.EqualTo(new[] { BaLo }));
            Assert.That(gio.LaySoTrenHuyHieu(), Is.EqualTo(1));
        });
    }

    // TC04
    [Test, Category("Smoke")]
    public void TronLuong_MotSanPham_DatHangThanhCong()
    {
        DangNhap().ThemVaoGio(BaLo);
        CheckoutOverviewPage tongKet = new CartPage(driver).MoGioHang()
            .BamThanhToan()
            .DienVaTiepTuc("Nguyen", "An", "70000");

        Assert.Multiple(() =>
        {
            Assert.That(tongKet.LayTienHang(), Is.EqualTo(29.99m));
            Assert.That(tongKet.LayTienThue(), Is.EqualTo(2.40m));
            Assert.That(tongKet.LayTongTien(), Is.EqualTo(32.39m));
        });

        tongKet.BamFinish();

        Assert.Multiple(() =>
        {
            Assert.That(tongKet.LayTieuDeHoanTat(), Is.EqualTo("Thank you for your order!"));
            Assert.That(new CartPage(driver).LaySoTrenHuyHieu(), Is.EqualTo(0), "Đặt xong thì giỏ phải trống");
        });
    }

    // TC05 + TC07 + TC10 (đã sửa): không chép số tiền AI tự tính, mà cộng giá đọc được trong giỏ rồi so với trang Overview
    [TestCase(AoTreEm, TestName = "TongTien_MotSanPhamReNhat_DungBangGiaCong")]
    [TestCase(BaLo + "|" + DenXe + "|" + AoTreEm, TestName = "TongTien_BaSanPham_DungBangGiaCong")]
    [TestCase("TatCa", TestName = "TongTien_SauSanPham_DungBangGiaCong")]
    public void TongTien_BangTongGiaTrongGio_CongThue(string danhSach)
    {
        InventoryPage trangSanPham = DangNhap();
        if (danhSach == "TatCa") trangSanPham.ThemTatCaVaoGio();
        else foreach (string ten in danhSach.Split('|')) trangSanPham.ThemVaoGio(ten);

        CartPage gio = new CartPage(driver).MoGioHang();
        decimal tongGiaTrongGio = gio.LayDanhSachTenSanPham().Sum(gio.LayGia);
        int soSanPham = gio.LayDanhSachTenSanPham().Count;

        CheckoutOverviewPage tongKet = gio.BamThanhToan().DienVaTiepTuc("Nguyen", "An", "70000");
        decimal tienHang = tongKet.LayTienHang();
        decimal thue = tongKet.LayTienThue();

        Assert.Multiple(() =>
        {
            Assert.That(tongKet.DemSoSanPham(), Is.EqualTo(soSanPham));
            Assert.That(tienHang, Is.EqualTo(tongGiaTrongGio), "Item total phải bằng tổng giá trong giỏ");
            Assert.That(thue, Is.EqualTo(Math.Round(tienHang * 0.08m, 2)), "Thuế của saucedemo là 8% tiền hàng");
            Assert.That(tongKet.LayTongTien(), Is.EqualTo(tienHang + thue), "Total = Item total + Tax");
        });
    }

    // TC06
    [Test]
    public void ContinueShopping_QuayVeTrangSanPham_GioGiuNguyen()
    {
        DangNhap().ThemVaoGio(BaLo);
        InventoryPage trangSanPham = new CartPage(driver).MoGioHang().TiepTucMuaSam();

        Assert.Multiple(() =>
        {
            Assert.That(trangSanPham.DaHienThi(), Is.True, "Continue Shopping phải về trang sản phẩm");
            Assert.That(trangSanPham.LayChuNutCuaSanPham(BaLo), Is.EqualTo("Remove"), "Sản phẩm đã thêm vẫn còn trong giỏ");
        });

        trangSanPham.ThemVaoGio(DenXe);
        CartPage gio = new CartPage(driver).MoGioHang();

        Assert.That(gio.LayDanhSachTenSanPham(), Is.EqualTo(new[] { BaLo, DenXe }));
    }

    // TC08
    [Test]
    public void XoaSanPhamCuoiCung_GioTrong_HuyHieuMat()
    {
        DangNhap().ThemVaoGio(BaLo);
        CartPage gio = new CartPage(driver).MoGioHang().XoaSanPham(BaLo);

        Assert.Multiple(() =>
        {
            Assert.That(gio.LayDanhSachTenSanPham(), Is.Empty);
            Assert.That(gio.LaySoTrenHuyHieu(), Is.EqualTo(0));
        });
    }

    // TC09
    [Test]
    public void ThemRoiBoNgayTrenTrangSanPham_HuyHieuMat()
    {
        InventoryPage trangSanPham = DangNhap().ThemVaoGio(BaLo).BoKhoiGio(BaLo);

        Assert.Multiple(() =>
        {
            Assert.That(new CartPage(driver).LaySoTrenHuyHieu(), Is.EqualTo(0));
            Assert.That(trangSanPham.LayChuNutCuaSanPham(BaLo), Is.EqualTo("Add to cart"));
        });
    }

    // TC13
    [Test]
    public void Checkout_BoTrongLastName_BaoLoi()
    {
        DangNhap().ThemVaoGio(BaLo);
        CheckoutPage thongTin = new CartPage(driver).MoGioHang()
            .BamThanhToan()
            .DienThongTin("Nguyen", "", "70000")
            .BamTiepTuc();

        Assert.Multiple(() =>
        {
            Assert.That(thongTin.DaSangBuocHai(), Is.False);
            Assert.That(thongTin.LayThongBaoLoi(), Is.EqualTo("Error: Last Name is required"));
        });
    }

    // TC16
    [Test]
    public void CancelOTrangOverview_VeTrangSanPham_GioVanCon()
    {
        DangNhap().ThemVaoGio(BaLo);
        InventoryPage trangSanPham = new CartPage(driver).MoGioHang()
            .BamThanhToan()
            .DienVaTiepTuc("Nguyen", "An", "70000")
            .BamCancel();

        Assert.Multiple(() =>
        {
            Assert.That(trangSanPham.DaHienThi(), Is.True);
            Assert.That(new CartPage(driver).LaySoTrenHuyHieu(), Is.EqualTo(1));
        });
    }
}
