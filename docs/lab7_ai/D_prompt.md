Mình đang viết test Selenium C# (NUnit 4, Selenium.WebDriver 4.52) cho trang giỏ hàng của https://www.saucedemo.com. Có 2 test cứ chạy là rớt, cả hai đều gọi `CartPage.XoaSanPham`. Các test khác dùng cùng `CartPage` nhưng không gọi hàm này thì xanh hết. Bạn giúp mình tìm nguyên nhân và chỉ cách sửa nhé. Dự án không cho dùng `Thread.Sleep`. `Tim(locator)` của `BasePage` là `wait.Until(d => d.FindElement(locator))`, còn `wait` là `WebDriverWait` 10 giây.

**Stack trace:**
```
XoaSanPhamCuoiCung_GioTrong_HuyHieuMat
OpenQA.Selenium.WebDriverTimeoutException : Timed out after 10 seconds
  at OpenQA.Selenium.Support.UI.DefaultWait`1.ThrowTimeoutException(String exceptionMessage, Exception lastException)
  at OpenQA.Selenium.Support.UI.DefaultWait`1.Until[TResult](Func`2 condition, CancellationToken token)
  at OpenQA.Selenium.Support.UI.DefaultWait`1.Until[TResult](Func`2 condition)
  at FirstTests.Pages.CartPage.XoaSanPham(String ten) in C:\...\FirstTests\Pages\CartPage.cs:line 71
  at FirstTests.Tests.GioHangTests.XoaSanPhamCuoiCung_GioTrong_HuyHieuMat() in C:\...\FirstTests\Tests\GioHangTests.cs:line 138
```

**Test bị rớt** (`GioHangTests.cs`):
```csharp
[Test]
public void XoaSanPhamCuoiCung_GioTrong_HuyHieuMat()
{
    DangNhap().ThemVaoGio(BaLo);
    CartPage gio = new CartPage(driver).MoGioHang().XoaSanPham(BaLo);   // dòng 138

    Assert.Multiple(() =>
    {
        Assert.That(gio.LayDanhSachTenSanPham(), Is.Empty);
        Assert.That(gio.LaySoTrenHuyHieu(), Is.EqualTo(0));
    });
}
```

**Phần liên quan trong `Pages/CartPage.cs`** (dòng 71 là `wait.Until(...)` trong `XoaSanPham`):
```csharp
private readonly By danhSachGio = By.CssSelector(".cart_list");
private readonly By dongSanPham = By.CssSelector(".cart_item");
private readonly By tenSanPham = By.CssSelector(".inventory_item_name");
private readonly By nutXoa = By.CssSelector("button.cart_button");

public CartPage XoaSanPham(string ten)
{
    var dong = TimDongTheoTen(ten);
    dong.FindElement(nutXoa).Click();
    wait.Until(_ => DaBiGoKhoiTrang(dong));
    return this;
}

private IReadOnlyCollection<IWebElement> LayCacDongSanPham() =>
    Tim(danhSachGio).FindElements(dongSanPham);

private IWebElement TimDongTheoTen(string ten) =>
    LayCacDongSanPham().FirstOrDefault(dong => dong.FindElement(tenSanPham).Text.Trim() == ten)
    ?? throw new NoSuchElementException($"Không có sản phẩm \"{ten}\" trong giỏ hàng.");

private static bool DaBiGoKhoiTrang(IWebElement phanTu)
{
    try
    {
        _ = phanTu.Displayed;
        return false;
    }
    catch (StaleElementReferenceException)
    {
        return true;
    }
}
```

Bạn trả lời giúp mình theo thứ tự: (1) nguyên nhân gốc, (2) code `XoaSanPham` đã sửa, (3) giải thích vì sao cách sửa đó chạy ổn định.
