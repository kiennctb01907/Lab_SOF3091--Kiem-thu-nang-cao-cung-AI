using System.Globalization;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace FirstTests.Lab3;

/// <summary>
/// Lab 3 - Phần B: chọn sắp xếp "Price (low to high)" bằng SelectElement
/// và Assert danh sách giá THỰC SỰ tăng dần.
/// </summary>
[TestFixture]
public class SortDropdownTests : Lab3TestBase
{
    private void Login(string username, string password)
    {
        driver.Navigate().GoToUrl("https://www.saucedemo.com/");
        driver.FindElement(By.Id("user-name")).SendKeys(username);
        driver.FindElement(By.Id("password")).SendKeys(password);
        driver.FindElement(By.Id("login-button")).Click();
        // Chờ trang inventory hiện danh sách sản phẩm
        wait.Until(d => d.FindElements(By.CssSelector("[data-test='inventory-item']")).Count > 0);
    }

    [Test]
    public void SortPriceLowToHigh_PricesShouldBeAscending()
    {
        Login("standard_user", "secret_sauce");

        var sort = new SelectElement(driver.FindElement(By.CssSelector("[data-test='product-sort-container']")));
        sort.SelectByText("Price (low to high)");

        List<decimal> prices = driver.FindElements(By.CssSelector("[data-test='inventory-item-price']"))
            .Select(e => decimal.Parse(e.Text.Replace("$", ""), CultureInfo.InvariantCulture))
            .ToList();
        Console.WriteLine("Giá sau khi sắp xếp: " + string.Join(", ", prices));

        // So sánh danh sách vừa lấy với chính nó sau khi đã sắp xếp tăng dần
        List<decimal> sorted = prices.OrderBy(p => p).ToList();

        // Sau khi chọn, trang vẽ lại dropdown nên phải tìm lại phần tử (tránh StaleElementReferenceException)
        var sortAfter = new SelectElement(driver.FindElement(By.CssSelector("[data-test='product-sort-container']")));

        Assert.Multiple(() =>
        {
            Assert.That(sortAfter.SelectedOption.Text, Is.EqualTo("Price (low to high)"), "Dropdown phải đang chọn Price (low to high)");
            Assert.That(prices, Has.Count.EqualTo(6), "Phải có 6 giá");
            Assert.That(prices, Is.EqualTo(sorted), "Danh sách giá phải tăng dần");
            Assert.That(prices, Is.Ordered.Ascending, "Danh sách giá phải tăng dần");
            Assert.That(prices.First(), Is.EqualTo(prices.Min()), "Giá đầu tiên phải là giá nhỏ nhất");
            Assert.That(prices.Last(), Is.EqualTo(prices.Max()), "Giá cuối cùng phải là giá lớn nhất");
        });
    }
}
