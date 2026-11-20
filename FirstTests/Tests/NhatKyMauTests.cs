using FirstTests.Core;
using FirstTests.Pages;

namespace FirstTests.Tests;

/// <summary>
/// Lab 6 - Phần B: chạy riêng class này để có một lần chạy gồm cả ca ĐẬU và ca RỚT, xem nhật ký BaseTest ghi ra.
/// Gắn [Explicit] ở mức class để Run All và các lần đo song song không bị ca cố ý rớt làm đỏ.
/// File này không gọi Log lần nào: dòng BẮT ĐẦU / OK / RỚT đều do BaseTest ghi.
/// </summary>
[TestFixture]
[Explicit("Lab 6 - Phần B: minh họa nhật ký có ca đậu và ca rớt")]
[Category("NhatKyMau")]
public class NhatKyMauTests : BaseTest
{
    [Test]
    public void DangNhapDung_VaoTrangSanPham_Dau()
    {
        InventoryPage trangSanPham = new LoginPage(driver).DangNhap("standard_user", "secret_sauce");

        Assert.That(trangSanPham.DaHienThi(), Is.True);
    }

    [Test]
    public void TaiKhoanBiKhoa_CoYKyVongVaoDuoc_Rot()
    {
        InventoryPage trangSanPham = new LoginPage(driver).DangNhap("locked_out_user", "secret_sauce");

        // Cố ý sai: tài khoản bị khóa thì KHÔNG vào được trang sản phẩm
        Assert.That(trangSanPham.DaHienThi(), Is.True, "Cố ý sai để có dòng [ERROR] trong nhật ký");
    }
}
