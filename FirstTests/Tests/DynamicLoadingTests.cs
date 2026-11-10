using FirstTests.Pages;

namespace FirstTests.Tests;

/// <summary>
/// Lab 3 - Phần A, viết lại bằng Page Object Model: chứng minh vấn đề đồng bộ trên trang dynamic_loading/2.
/// Phần tử "Hello World!" chỉ được JavaScript tạo ra vài giây SAU khi bấm Start.
/// </summary>
[TestFixture]
public class DynamicLoadingTests : TestBase
{
    /// <summary>
    /// Bước 1: đọc lời chào NGAY sau khi bấm Start, không chờ -> rớt với NoSuchElementException.
    /// Gắn [Explicit] để Run All vẫn xanh; muốn xem lại lỗi thì chạy riêng test này.
    /// </summary>
    [Test, Explicit("Phần A bước 1: cố ý rớt để chứng minh vấn đề đồng bộ")]
    public void Step1_FindFinishWithoutWait_Fails()
    {
        DynamicLoadingPage trang = new DynamicLoadingPage(driver).Mo().BamStart();

        string loiChao = trang.LayLoiChaoNgayKhongCho();

        Assert.That(loiChao, Is.EqualTo("Hello World!"));
    }

    /// <summary>Bước 2: thêm Explicit Wait (nằm trong Page Object) -> pass.</summary>
    [Test]
    public void Step2_FindFinishWithExplicitWait_Passes()
    {
        DynamicLoadingPage trang = new DynamicLoadingPage(driver).Mo().BamStart();

        string loiChao = trang.LayLoiChaoSauKhiChoHienRa();
        Console.WriteLine($"Nội dung #finish: {loiChao}");

        Assert.That(loiChao, Is.EqualTo("Hello World!"));
    }
}
