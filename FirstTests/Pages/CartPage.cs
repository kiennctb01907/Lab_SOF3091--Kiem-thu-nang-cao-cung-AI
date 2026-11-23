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
    /// Lab 7 - Phần D: bản AI viết đầu tiên chờ dòng cũ bị gỡ khỏi DOM (stale) nên luôn timeout,
    /// vì saucedemo dùng lại thẻ div cũ và chỉ đổi class thành removed_cart_item.
    /// Nay chờ đến khi không còn dòng .cart_item nào mang tên này, mỗi vòng chờ tìm lại từ đầu.
    /// </summary>
    public CartPage XoaSanPham(string ten)
    {
        TimDongTheoTen(ten).FindElement(nutXoa).Click();
        wait.Until(_ => !ConSanPhamTrongGio(ten));
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

    /// <summary>Còn dòng nào trong giỏ mang tên này không. DOM đang đổi giữa chừng thì coi như chưa xong để vòng chờ sau kiểm lại.</summary>
    private bool ConSanPhamTrongGio(string ten)
    {
        try
        {
            return driver.FindElements(dongSanPham)
                         .Any(dong => dong.FindElement(tenSanPham).Text.Trim() == ten);
        }
        catch (StaleElementReferenceException)
        {
            return true;
        }
        catch (NoSuchElementException)
        {
            return true;
        }
    }
}
