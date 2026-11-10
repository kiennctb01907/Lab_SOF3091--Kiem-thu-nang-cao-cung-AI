using System.Globalization;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace FirstTests.Pages;

/// <summary>Trang danh sách sản phẩm sau khi đăng nhập — nghiệp vụ: xem, sắp xếp, đọc tên và giá.</summary>
public class InventoryPage : BasePage
{
    public const string Url = LoginPage.Url + "inventory.html";

    private readonly By danhSachSanPham = By.CssSelector("[data-test='inventory-list']");
    private readonly By moiSanPham = By.CssSelector("[data-test='inventory-item']");
    private readonly By tenSanPham = By.CssSelector("[data-test='inventory-item-name']");
    private readonly By giaSanPham = By.CssSelector("[data-test='inventory-item-price']");
    private readonly By oChonSapXep = By.CssSelector("[data-test='product-sort-container']");

    public InventoryPage(IWebDriver driver) : base(driver) { }

    /// <summary>Người dùng có nhìn thấy danh sách sản phẩm hay không (chờ tối đa 10 giây).</summary>
    public bool DaHienThi()
    {
        try
        {
            return wait.Until(d => d.FindElement(danhSachSanPham).Displayed);
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }

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
