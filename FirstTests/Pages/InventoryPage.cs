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
            return TimHienThi(danhSachSanPham).Displayed;
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

    /// <summary>Người dùng đọc tên và giá của từng sản phẩm đang hiển thị, theo đúng thứ tự trên màn hình.</summary>
    public List<SanPham> LayDanhSachSanPham()
    {
        var ketQua = new List<SanPham>();
        foreach (IWebElement item in TimTatCa(moiSanPham))
        {
            // Lấy thông tin BÊN TRONG từng sản phẩm bằng item.FindElement
            ketQua.Add(new SanPham(
                item.FindElement(tenSanPham).Text,
                item.FindElement(giaSanPham).Text));
        }
        return ketQua;
    }

    /// <summary>Người dùng đọc giá (dạng số) của các sản phẩm đang hiển thị, theo đúng thứ tự trên màn hình.</summary>
    public List<decimal> LayDanhSachGia() => LayDanhSachSanPham().Select(p => p.Gia).ToList();

    /// <summary>Người dùng đọc giá (đúng chữ trên màn hình) của sản phẩm đầu tiên.</summary>
    public string LayGiaSanPhamDauTien() => TimTatCa(giaSanPham).First().Text;
}
