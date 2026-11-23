```csharp
using System.Globalization;
using OpenQA.Selenium;

namespace FirstTests.Pages;

/// <summary>
/// Trang giỏ hàng saucedemo (cart.html). Nghiệp vụ: mở giỏ, đọc tên/giá/số lượng sản phẩm,
/// đọc huy hiệu giỏ, xóa sản phẩm, quay lại mua tiếp hoặc sang bước Checkout.
/// </summary>
public class CartPage : BasePage
{
    // Phần đầu trang: dùng chung cho mọi trang sau khi đăng nhập
    private readonly By bieuTuongGio = By.CssSelector(".shopping_cart_link");
    private readonly By huyHieuGio = By.CssSelector(".shopping_cart_badge");

    // Phần thân trang giỏ hàng
    private readonly By danhSachGio = By.CssSelector(".cart_list");
    private readonly By dongSanPham = By.CssSelector(".cart_item");
    private readonly By tenSanPham = By.CssSelector(".inventory_item_name");
    private readonly By giaSanPham = By.CssSelector(".inventory_item_price");
    private readonly By soLuongSanPham = By.CssSelector(".cart_quantity");
    private readonly By nutXoa = By.CssSelector("button.cart_button");
    private readonly By nutTiepTucMua = By.Id("continue-shopping");
    private readonly By nutThanhToan = By.Id("checkout");

    public CartPage(IWebDriver driver) : base(driver) { }

    /// <summary>
    /// Người dùng bấm biểu tượng giỏ hàng ở góc trên. Dùng được từ bất kỳ trang nào sau khi đăng nhập.
    /// Chờ đến khi đã sang cart.html và danh sách giỏ đã có trong DOM.
    /// </summary>
    public CartPage MoGioHang()
    {
        Tim(bieuTuongGio).Click();
        wait.Until(d => d.Url.Contains("cart.html"));
        Tim(danhSachGio);
        return this;
    }

    /// <summary>Người dùng đọc tên các sản phẩm đang có trong giỏ, theo thứ tự hiển thị. Giỏ rỗng thì trả danh sách rỗng.</summary>
    public IReadOnlyList<string> LayDanhSachTenSanPham() =>
        LayCacDongSanPham()
            .Select(dong => dong.FindElement(tenSanPham).Text.Trim())
            .ToList();

    /// <summary>Người dùng đọc giá của một sản phẩm trong giỏ (ví dụ "$29.99" thì trả 29.99).</summary>
    public decimal LayGia(string ten)
    {
        var chuoiGia = TimDongTheoTen(ten).FindElement(giaSanPham).Text.Trim().TrimStart('$');
        return decimal.Parse(chuoiGia, NumberStyles.Number, CultureInfo.InvariantCulture);
    }

    /// <summary>Người dùng đọc số lượng của một sản phẩm trong giỏ.</summary>
    public int LaySoLuong(string ten) =>
        int.Parse(TimDongTheoTen(ten).FindElement(soLuongSanPham).Text.Trim(), CultureInfo.InvariantCulture);

    /// <summary>Người dùng đọc số trên huy hiệu giỏ hàng. Không có huy hiệu (giỏ rỗng) thì trả 0, không ném lỗi.</summary>
    public int LaySoTrenHuyHieu() =>
        CoHienThi(huyHieuGio)
            ? int.Parse(Tim(huyHieuGio).Text.Trim(), CultureInfo.InvariantCulture)
            : 0;

    /// <summary>
    /// Người dùng bấm "Remove" ở dòng sản phẩm có tên tương ứng.
    /// Chờ đến khi dòng đó biến mất khỏi DOM rồi mới trả về, để lần đọc tiếp theo không thấy dữ liệu cũ.
    /// </summary>
    public CartPage XoaSanPham(string ten)
    {
        var dong = TimDongTheoTen(ten);
        dong.FindElement(nutXoa).Click();
        wait.Until(_ => DaBiGoKhoiTrang(dong));
        return this;
    }

    /// <summary>Người dùng bấm "Continue Shopping" để quay về trang sản phẩm.</summary>
    public InventoryPage TiepTucMuaSam()
    {
        Tim(nutTiepTucMua).Click();
        return new InventoryPage(driver);
    }

    /// <summary>Người dùng bấm "Checkout" để sang trang nhập thông tin (Your Information).</summary>
    public CheckoutPage BamThanhToan()
    {
        Tim(nutThanhToan).Click();
        return new CheckoutPage(driver);
    }

    // ---------------- Hàm hỗ trợ nội bộ (private, không lộ IWebElement ra ngoài) ----------------

    /// <summary>
    /// Lấy các dòng sản phẩm. Chờ khung .cart_list bằng Tim() trước (khung này luôn có, kể cả khi giỏ rỗng),
    /// sau đó tìm các dòng trong khung. FindElements trả danh sách rỗng thay vì ném lỗi khi giỏ rỗng.
    /// </summary>
    private IReadOnlyCollection<IWebElement> LayCacDongSanPham() =>
        Tim(danhSachGio).FindElements(dongSanPham);

    /// <summary>Tìm dòng sản phẩm theo tên hiển thị (so khớp chính xác, bỏ khoảng trắng hai đầu).</summary>
    private IWebElement TimDongTheoTen(string ten) =>
        LayCacDongSanPham().FirstOrDefault(dong => dong.FindElement(tenSanPham).Text.Trim() == ten)
        ?? throw new NoSuchElementException($"Không có sản phẩm \"{ten}\" trong giỏ hàng.");

    /// <summary>Phần tử đã bị gỡ khỏi DOM thì mọi truy cập vào nó ném StaleElementReferenceException.</summary>
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
}
```

**Các locator đã dùng và lý do chọn**

| Locator | Phần tử | Lý do |
|---|---|---|
| `.shopping_cart_link` | Biểu tượng giỏ ở góc trên | Header nào cũng có, class tồn tại từ lâu và không đổi theo trạng thái giỏ |
| `.shopping_cart_badge` | Huy hiệu số lượng | Chỉ được render khi giỏ có hàng. Vì vậy hỏi bằng `CoHienThi` để trả 0 mà không ném lỗi |
| `.cart_list` | Khung danh sách giỏ | Luôn có trong DOM, kể cả khi giỏ rỗng, nên dùng làm "mốc" cho `Tim()` chờ trang sẵn sàng |
| `.cart_item` | Một dòng sản phẩm | Gom tên, giá, số lượng và nút Remove của cùng một sản phẩm, nên tìm con bên trong theo phạm vi dòng thay vì ghép nhiều danh sách theo chỉ số |
| `.inventory_item_name` / `.inventory_item_price` / `.cart_quantity` | Tên, giá, số lượng | Class ngữ nghĩa và ổn định. Chỉ tìm trong phạm vi `.cart_item` nên không bị lẫn với dòng khác |
| `button.cart_button` (trong dòng) | Nút Remove | Id của nút phụ thuộc tên sản phẩm (`remove-sauce-labs-backpack`). Tìm theo phạm vi dòng giúp khỏi tự chuyển tên thành id, và dùng được cho mọi sản phẩm |
| `By.Id("continue-shopping")`, `By.Id("checkout")` | Hai nút điều hướng | Id là duy nhất và nhanh nhất, giống cách `LoginPage` dùng `By.Id` |

**Ghi chú cho người review:**
- Việc đọc danh sách dùng `FindElements` *bên trong* phần tử đã lấy bằng `Tim()`, không gọi `driver.FindElement` trực tiếp. Lý do: `Tim()` chỉ trả về một phần tử, mà giỏ có thể rỗng.
- Tên sản phẩm được so khớp trong C# thay vì ghép vào XPath. Cách này tránh lỗi khi tên chứa dấu nháy hoặc ký tự đặc biệt, ví dụ `Test.allTheThings() T-Shirt (Red)`.
- `XoaSanPham` chờ dòng bị gỡ khỏi DOM (stale) thay vì dùng `Thread.Sleep`.
- Giá được parse bằng `CultureInfo.InvariantCulture`. Nếu không, máy đặt locale vi-VN sẽ hiểu dấu `.` sai.
- Biểu tượng giỏ và huy hiệu thực chất thuộc phần header chung. Về lâu dài nên tách ra một `HeaderComponent` hoặc đưa lên `BasePage`. Ở đây vẫn để trong `CartPage` vì đề bài yêu cầu chỉ một file.
- Code giả định `InventoryPage` và `CheckoutPage` có constructor `(IWebDriver driver)` theo quy ước 1.
