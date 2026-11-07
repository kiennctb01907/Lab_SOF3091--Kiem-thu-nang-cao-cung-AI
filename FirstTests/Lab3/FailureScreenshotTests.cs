using OpenQA.Selenium;

namespace FirstTests.Lab3;

/// <summary>
/// Lab 3 - Phần D: test CỐ Ý rớt để kiểm tra cơ chế chụp ảnh trong [TearDown] của Lab3TestBase.
/// Gắn [Explicit] để Run All vẫn xanh; chạy riêng test này sẽ sinh ảnh FailureScreenshots/{tên test}.png.
/// </summary>
[TestFixture]
public class FailureScreenshotTests : Lab3TestBase
{
    [Test, Explicit("Phần D: cố ý rớt để sinh ảnh chụp khi Failed")]
    public void InventoryFirstPrice_DeliberatelyWrongExpectation_Fails()
    {
        driver.Navigate().GoToUrl("https://www.saucedemo.com/");
        driver.FindElement(By.Id("user-name")).SendKeys("standard_user");
        driver.FindElement(By.Id("password")).SendKeys("secret_sauce");
        driver.FindElement(By.Id("login-button")).Click();

        IWebElement firstPrice = wait.Until(d => d.FindElement(By.CssSelector("[data-test='inventory-item-price']")));

        // Cố ý sai: giá đầu tiên của trang mặc định là $29.99, không phải $7.99
        Assert.That(firstPrice.Text, Is.EqualTo("$7.99"), "Cố ý sai để test rớt và sinh ảnh chụp màn hình");
    }
}
