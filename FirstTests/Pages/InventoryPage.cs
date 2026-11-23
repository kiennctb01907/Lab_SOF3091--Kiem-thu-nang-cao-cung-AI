using System.Globalization;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace FirstTests.Pages;

/// <summary>Trang danh sách sản phẩm sau khi đăng nhập — nghiệp vụ: xem, sắp xếp, đọc tên và giá.</summary>
public class InventoryPage : BasePage
{
    private readonly By danhSachSanPham = By.CssSelector("[data-test='inventory-list']");
    private readonly By moiSanPham = By.CssSelector("[data-test='inventory-item']");
    private readonly By tenSanPham = By.CssSelector("[data-test='inventory-item-name']");
    private readonly By giaSanPham = By.CssSelector("[data-test='inventory-item-price']");
    private readonly By oChonSapXep = By.CssSelector("[data-test='product-sort-container']");

    public InventoryPage(IWebDriver driver) : base(driver) { }

    /// <summary>Địa chỉ trang sản phẩm, ghép từ BaseUrl trong config.</summary>
    public string Url => cfg.BaseUrl + "inventory.html";

    /// <summary>
    /// Người dùng có vào được trang sản phẩm hay không. Trả về false (không ném lỗi) khi đăng nhập thất bại,
    /// chỉ chờ CheckTimeoutSeconds giây để ca nghịch không phải chờ hết TimeoutSeconds.
    /// </summary>
    public bool DaHienThi() => CoHienThi(danhSachSanPham);

    /// <summary>Người dùng chọn kiểu sắp xếp theo value của ô chọn (az, za, lohi, hilo).</summary>
    public InventoryPage SapXep(string value)
    {
        new SelectElement(Tim(oChonSapXep)).SelectByValue(value);
        return this;
    }

    /// <summary>Người dùng đọc kiểu sắp xếp đang được chọn (ví dụ "Price (low to high)").</summary>
    public string LayKieuSapXepDangChon()
    {
        // Tìm lại ô chọn mỗi lần gọi vì sau khi chọn trang vẽ lại ô này (tránh StaleElementReferenceException)
        return new SelectElement(Tim(oChonSapXep)).SelectedOption.Text;
    }

    /// <summary>Người dùng đọc tên của từng sản phẩm đang hiển thị, theo thứ tự trên màn hình.</summary>
    public List<string> LayDanhSachTenSanPham() => LayChuTrongTungSanPham(tenSanPham);

    /// <summary>Người dùng đọc giá của từng sản phẩm đúng như chữ trên màn hình (ví dụ "$29.99").</summary>
    public List<string> LayDanhSachGiaHienThi() => LayChuTrongTungSanPham(giaSanPham);

    /// <summary>Người dùng đọc giá (dạng số) của từng sản phẩm, theo thứ tự trên màn hình.</summary>
    public List<decimal> LayDanhSachGia() => LayDanhSachGiaHienThi()
        .Select(gia => decimal.Parse(gia.TrimStart('$'), CultureInfo.InvariantCulture))
        .ToList();

    /// <summary>Người dùng đọc giá (đúng chữ trên màn hình) của sản phẩm đầu tiên.</summary>
    public string LayGiaSanPhamDauTien() => LayDanhSachGiaHienThi().First();

    /// <summary>Địa chỉ trang người dùng đang đứng.</summary>
    public string LayUrl() => driver.Url;

    /// <summary>Lab 7: người dùng bấm nút "Add to cart" trên thẻ của sản phẩm có tên này.</summary>
    public InventoryPage ThemVaoGio(string tenSanPhamCanThem)
    {
        BamNutTrenThe(tenSanPhamCanThem, "Add to cart");
        return this;
    }

    /// <summary>Lab 7: người dùng bấm nút "Remove" ngay trên thẻ sản phẩm (không vào giỏ).</summary>
    public InventoryPage BoKhoiGio(string tenSanPhamCanBo)
    {
        BamNutTrenThe(tenSanPhamCanBo, "Remove");
        return this;
    }

    /// <summary>Lab 7: người dùng đọc chữ trên nút của thẻ sản phẩm ("Add to cart" hoặc "Remove").</summary>
    public string LayChuNutCuaSanPham(string ten) => wait.Until(d => DocChuNut(d, ten));

    /// <summary>Lab 7: người dùng thêm toàn bộ sản phẩm đang hiển thị vào giỏ.</summary>
    public InventoryPage ThemTatCaVaoGio()
    {
        foreach (string ten in LayDanhSachTenSanPham()) ThemVaoGio(ten);
        return this;
    }

    private readonly By nutTrenThe = By.CssSelector("button.btn_inventory");

    /// <summary>Tìm thẻ sản phẩm theo tên, bấm nút trên thẻ rồi chờ nút đổi chữ (trang vẽ lại nút sau khi bấm).</summary>
    private void BamNutTrenThe(string ten, string chuNutTruocKhiBam)
    {
        IWebElement nut = wait.Until(d => TheCuaSanPham(d, ten)?.FindElement(nutTrenThe))!;
        if (nut.Text != chuNutTruocKhiBam)
        {
            throw new InvalidOperationException($"Nút của \"{ten}\" đang là \"{nut.Text}\", không phải \"{chuNutTruocKhiBam}\".");
        }
        nut.Click();
        wait.Until(d => DocChuNut(d, ten) is string chu && chu != chuNutTruocKhiBam);
    }

    /// <summary>Đọc chữ trên nút của thẻ sản phẩm; nút vừa bị vẽ lại thì trả null để vòng chờ đọc lại.</summary>
    private string? DocChuNut(IWebDriver d, string ten)
    {
        try
        {
            return TheCuaSanPham(d, ten)?.FindElement(nutTrenThe).Text;
        }
        catch (StaleElementReferenceException)
        {
            return null;
        }
    }

    /// <summary>
    /// Thẻ sản phẩm theo tên. Trang vừa đăng nhập xong có thể vẽ lại danh sách giữa chừng (StaleElementReferenceException),
    /// lúc đó trả null để vòng chờ của wait.Until thử lại thay vì làm test rớt ngẫu nhiên.
    /// </summary>
    private IWebElement? TheCuaSanPham(IWebDriver d, string ten)
    {
        try
        {
            return d.FindElements(moiSanPham).FirstOrDefault(item => item.FindElement(tenSanPham).Text == ten);
        }
        catch (StaleElementReferenceException)
        {
            return null;
        }
    }

    /// <summary>Chờ có sản phẩm rồi lấy chữ BÊN TRONG từng sản phẩm bằng item.FindElement.</summary>
    private List<string> LayChuTrongTungSanPham(By locatorBenTrong)
    {
        IReadOnlyCollection<IWebElement> cacSanPham = wait.Until(d =>
        {
            IReadOnlyCollection<IWebElement> ds = d.FindElements(moiSanPham);
            return ds.Count > 0 ? ds : null;
        })!;
        return cacSanPham.Select(item => item.FindElement(locatorBenTrong).Text).ToList();
    }
}
