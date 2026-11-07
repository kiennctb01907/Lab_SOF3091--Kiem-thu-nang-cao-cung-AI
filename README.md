# Lab 3 — Xử lý các tình huống thực tế phức tạp

Làm tiếp trên repo của Lab 1–2. Code Lab 3 nằm trong `FirstTests/Lab3/`, dùng lớp cơ sở `Lab3TestBase` chỉ có **Explicit Wait** (`WebDriverWait`, 10 giây), không bật Implicit Wait, không dùng `Thread.Sleep`. Đã thêm gói `Selenium.Support`.

## Phần A — Chứng minh vấn đề đồng bộ (`DynamicLoadingTests.cs`)

Trang: https://the-internet.herokuapp.com/dynamic_loading/2 — bấm **Start** thì vài giây sau JavaScript mới tạo ra phần tử `#finish` chứa chữ "Hello World!".

| Bước | Test | Kết quả | Ảnh |
|---|---|---|---|
| 1 — không chờ | `Step1_FindFinishWithoutWait_Fails`: tìm `#finish` ngay sau khi bấm Start | **Rớt**, `NoSuchElementException: no such element: Unable to locate element: {"method":"css selector","selector":"#finish"}` | `screenshots/lab3_A1_no_wait_failed.png` |
| 2 — Explicit Wait | `Step2_FindFinishWithExplicitWait_Passes`: `wait.Until(d => ...FindElement(By.Id("finish")) ...)` | **Pass**, đọc được "Hello World!" | `screenshots/lab3_A2_explicit_wait_passed.png` |
| Kiểm tra lại | Chạy bước 2 liên tiếp 10 lần | 10/10 Passed | `screenshots/lab3_A3_explicit_wait_10_runs.png` |

Test bước 1 được gắn `[Explicit]` để Run All vẫn xanh; muốn xem lại lỗi thì chạy riêng: `dotnet test --filter Name=Step1_FindFinishWithoutWait_Fails`.

**Vì sao cùng một code mà kết quả khác nhau?** Selenium chỉ tự chờ trang tải xong khi chuyển trang, còn phần tử `#finish` thì được JavaScript tạo ra vài giây sau khi bấm Start, lúc trang đã tải xong từ lâu. Ở bước 1, `FindElement` chạy ngay lập tức khi `#finish` chưa tồn tại nên ném `NoSuchElementException`; kết quả phụ thuộc vào tốc độ mạng và máy, nên kiểu test này có thể lúc rớt lúc qua (flaky). Ở bước 2, `WebDriverWait.Until(...)` hỏi lại DOM liên tục tới khi `#finish` xuất hiện (tối đa 10 giây), nên test luôn đợi đúng lúc phần tử có mặt và pass ổn định.

## Phần B — Dropdown & Assert đúng nghiệp vụ (`SortDropdownTests.cs`)

Đăng nhập saucedemo bằng `standard_user`, dùng `SelectElement` chọn **Price (low to high)** trên dropdown `product_sort_container`, lấy 6 giá bằng `FindElements`, bỏ ký tự `$` rồi chuyển sang `decimal`. Kết quả in ra: `7.99, 9.99, 15.99, 15.99, 29.99, 49.99`.

Assert không chỉ đếm số lượng:
- Danh sách giá bằng **chính nó sau khi sắp xếp tăng dần** (`prices` == `prices.OrderBy(p => p)`), và `Is.Ordered.Ascending`.
- Giá đầu là nhỏ nhất, giá cuối là lớn nhất; dropdown đang chọn đúng "Price (low to high)".

Lỗi gặp khi làm: đọc lại lựa chọn dropdown sau khi sắp xếp bị `StaleElementReferenceException` vì trang vẽ lại dropdown — sửa bằng cách tìm lại phần tử. Ảnh: `screenshots/lab3_B_sort_price_passed.png`.

## Phần C — Alert & iframe (`AlertIframeTests.cs`)

Trang https://the-internet.herokuapp.com/javascript_alerts — chờ alert bằng `wait.Until(d => d.SwitchTo().Alert())`, Assert `alert.Text` **trước** khi xử lý, sau đó Assert `#result`:

| Ca | Thao tác | Assert `alert.Text` | Assert `#result` |
|---|---|---|---|
| JS Alert | `Accept()` | `I am a JS Alert` | `You successfully clicked an alert` |
| JS Confirm | `Dismiss()` | `I am a JS Confirm` | `You clicked: Cancel` |
| JS Prompt | `SendKeys("Xin chao Selenium")` + `Accept()` | `I am a JS prompt` | `You entered: Xin chao Selenium` |

Trang https://the-internet.herokuapp.com/iframe — `SwitchTo().Frame("mce_0_ifr")`, chờ TinyMCE khởi tạo xong, chọn hết (Ctrl+A) rồi gõ đoạn chữ vào trình soạn thảo, Assert nội dung đã gõ; sau đó `SwitchTo().DefaultContent()` và đọc được tiêu đề trang chính.

Lỗi gặp khi làm: lần đầu chữ không vào được trình soạn thảo vì test vào iframe khi TinyMCE chưa khởi tạo xong và `Clear()` không dùng được với vùng `contenteditable` — sửa bằng cách chờ thân trình soạn thảo sẵn sàng và dùng Ctrl+A. Ảnh: `screenshots/lab3_C_alert_iframe_passed.png`.

## Phần D — Chụp ảnh khi test thất bại (`Lab3TestBase.cs`)

Trong `[TearDown]` của lớp cơ sở (dùng chung cho mọi test Lab 3):

```csharp
if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed && driver != null)
{
    string dir = Path.Combine(TestContext.CurrentContext.WorkDirectory, "FailureScreenshots");
    Directory.CreateDirectory(dir);
    string path = Path.Combine(dir, $"{TestContext.CurrentContext.Test.Name}.png");
    ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile(path);
    TestContext.AddTestAttachment(path, "Ảnh chụp màn hình lúc test thất bại");
}
```

- Chỉ chụp khi `Outcome.Status == TestStatus.Failed` (cần `using NUnit.Framework.Interfaces;`); test pass thì không sinh ảnh.
- Tên file ảnh = tên ca test; đính kèm vào kết quả bằng `TestContext.AddTestAttachment(path)`.
- Test cố ý rớt: `FailureScreenshotTests.InventoryFirstPrice_DeliberatelyWrongExpectation_Fails` (mong đợi giá đầu là `$7.99` nhưng thật là `$29.99`), gắn `[Explicit]`.
- Ảnh minh chứng: `screenshots/lab3_D_InventoryFirstPrice_DeliberatelyWrongExpectation_Fails.png` (ảnh do test tự chụp) và `screenshots/lab3_D_failed_test_console.png` (kết quả rớt kèm đường dẫn ảnh).


## Phần E — AI chẩn đoán flaky test

**Test chọn:** `DynamicLoadingTests.Step1_FindFinishWithoutWait_Fails` (tình huống Phần A). Trước khi hỏi, em chạy test này 10 lần liên tiếp: **rớt 10/10 lần**, mỗi lần 4–9 giây.

### Prompt đã dùng

> Em có một test Selenium C# (NUnit 4.3.2, Selenium.WebDriver 4.52.0, Chrome 157, .NET 10, Windows 11, chạy bằng dotnet test trên máy cá nhân, mạng wifi bình thường, không chạy song song) bị rớt. Code:
>
> ```csharp
> [SetUp]
> public void SetUp()
> {
>     driver = new ChromeDriver(options);
>     driver.Manage().Window.Maximize();
>     wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
>     // không bật ImplicitWait
> }
>
> [Test]
> public void Step1_FindFinishWithoutWait_Fails()
> {
>     driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/dynamic_loading/2");
>     driver.FindElement(By.CssSelector("#start button")).Click();
>
>     IWebElement finish = driver.FindElement(By.Id("finish"));
>
>     Assert.That(finish.Text, Is.EqualTo("Hello World!"));
> }
> ```
>
> Lỗi nguyên văn:
> `OpenQA.Selenium.NoSuchElementException : no such element: Unable to locate element: {"method":"css selector","selector":"#finish"} (Session info: chrome=157.0.8103.38); For documentation on this error, please visit: https://www.selenium.dev/documentation/webdriver/troubleshooting/errors#nosuchelementexception`
>
> Tần suất: em chạy 10 lần liên tiếp thì rớt cả 10 lần (mỗi lần khoảng 4–9 giây). Khi em mở trang bằng tay, bấm Start thì thấy hiện thanh loading vài giây rồi mới hiện chữ "Hello World!".
>
> Anh/chị chẩn đoán giúp em các nguyên nhân có thể khiến test này rớt hoặc chập chờn (liệt kê từng nguyên nhân), và cách sửa đúng ạ. Bài yêu cầu không được dùng Thread.Sleep.

### Câu trả lời của AI (Claude)

> ## Chẩn đoán
>
> Test rớt **10/10 lần** nên đây là lỗi xảy ra mọi lần, không phải test chập chờn (flaky). Nguyên nhân chính là test đang chạy đua với trang web và lần nào cũng thua.
>
> **Các nguyên nhân có thể:**
>
> 1. **Không chờ phần tử xuất hiện (nguyên nhân chính).** Ở `dynamic_loading/2`, thẻ `#finish` **chưa có trong DOM** lúc bấm Start. Trang hiện thanh loading vài giây rồi mới thêm `#finish` vào. `FindElement` được gọi ngay sau `Click()` nên không tìm thấy và ném `NoSuchElementException`. Lỗi ghi `css selector "#finish"` là bình thường, vì Selenium tự đổi `By.Id` sang CSS.
>
> 2. **ImplicitWait đang bằng 0.** Em không bật ImplicitWait, nên `FindElement` tìm đúng một lần rồi ném lỗi luôn. Biến `wait` đã tạo trong `SetUp` nhưng chưa được dùng ở đâu.
>
> 3. **Chờ sai điều kiện.** Nếu chỉ chờ phần tử *có trong DOM* thì vẫn có thể rớt: `#finish` có thể có rồi nhưng chưa hiển thị, khi đó `.Text` trả về chuỗi rỗng. Với `/dynamic_loading/1` thì `#finish` có sẵn nhưng bị ẩn, nên lỗi sẽ là assert sai chứ không phải NoSuchElement. Cần chờ **hiển thị và có chữ**.
>
> 4. **Timeout quá ngắn khi server chậm.** Sau khi sửa, test vẫn có thể thỉnh thoảng rớt với `WebDriverTimeoutException`. Lý do là the-internet.herokuapp.com đôi khi phản hồi chậm (server lâu không có truy cập phải khởi động lại, hoặc wifi chậm). Timeout 10 giây thường đủ, nhưng có thể tăng lên 15 giây.
>
> 5. **Bấm nút trước khi trang tải xong (ít gặp).** Khi đó nút Start có thể chưa sẵn sàng để bấm. Nên chờ nút bấm được rồi mới `Click()`.
>
> ## Cách sửa (dùng explicit wait, không dùng Thread.Sleep)
>
> ```csharp
> [Test]
> public void Step2_FindFinishWithExplicitWait_Passes()
> {
>     driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/dynamic_loading/2");
>
>     // Chờ nút Start hiển thị và bấm được
>     IWebElement start = wait.Until(d =>
>     {
>         var btn = d.FindElement(By.CssSelector("#start button"));
>         return (btn.Displayed && btn.Enabled) ? btn : null;
>     });
>     start.Click();
>
>     // Chờ #finish xuất hiện, hiển thị và có chữ
>     wait.IgnoreExceptionTypes(typeof(NoSuchElementException),
>                               typeof(StaleElementReferenceException));
>     IWebElement finish = wait.Until(d =>
>     {
>         var el = d.FindElement(By.Id("finish"));
>         return (el.Displayed && !string.IsNullOrWhiteSpace(el.Text)) ? el : null;
>     });
>
>     Assert.That(finish.Text, Is.EqualTo("Hello World!"));
> }
> ```
>
> **Lưu ý:**
> - `IgnoreExceptionTypes` giúp `Until` thử lại liên tục (mặc định mỗi 500 ms) thay vì dừng ngay ở lỗi `NoSuchElementException` đầu tiên.
> - Không nên dùng chung ImplicitWait và explicit wait, vì thời gian chờ có thể cộng dồn khó đoán. Em đang không bật ImplicitWait là đúng.
> - `ExpectedConditions` cũ đã bị bỏ khỏi Selenium. Nếu muốn dùng thì cài thêm gói `DotNetSeleniumExtras.WaitHelpers`, còn viết lambda như trên thì không cần.

### Tự kiểm chứng từng gợi ý

| # | Gợi ý của AI | Cách em kiểm chứng | Kết luận |
|---|---|---|---|
| 1 | Không chờ phần tử, `#finish` chưa có trong DOM lúc bấm Start | Mở trang /2, đếm thời gian từ lúc bấm Start: trước khi bấm không có `#finish`; khoảng **5,0 giây** sau mới xuất hiện | **Đúng** — đây là nguyên nhân chính |
| 2 | ImplicitWait = 0, biến `wait` tạo ra nhưng không dùng | Đọc lại code bước 1: đúng là không gọi `wait.Until`; bước 2 dùng `wait.Until` thì pass 10/10 | **Đúng** |
| 3 | Chỉ chờ "có trong DOM" chưa đủ, phải chờ hiển thị và có chữ; ở /1 `#finish` có sẵn nhưng ẩn | Ở /2, lúc `#finish` vừa xuất hiện đã hiển thị và có sẵn chữ "Hello World!". Ở /1, `#finish` có sẵn với `display: none` | **Đúng với /1, không ảnh hưởng tới /2** — vẫn nên kiểm tra `Displayed` cho chắc |
| 4 | Server chậm có thể gây `WebDriverTimeoutException`, nên tăng timeout lên 15 giây | Trong 10 lần chạy bước 2 + 5 lần chạy code của AI không lần nào timeout; `#finish` xuất hiện sau khoảng 5 giây, còn dư nhiều so với 10 giây | **Có thể xảy ra nhưng chưa gặp** — giữ 10 giây |
| 5 | Bấm Start trước khi trang tải xong | `Navigate().GoToUrl()` đã tự chờ trang tải xong (pageLoadStrategy = normal); cả 10 lần rớt đều rớt ở dòng tìm `#finish`, chưa lần nào lỗi ở nút Start | **Không liên quan** tới lỗi này |
| — | Lưu ý: phải gọi `IgnoreExceptionTypes(typeof(NoSuchElementException))` thì `Until` mới thử lại | Code bước 2 của em **không** gọi `IgnoreExceptionTypes` mà vẫn pass 10/10, vì `WebDriverWait` của .NET mặc định đã bỏ qua `NotFoundException` (lớp cha của `NoSuchElementException`) | **Sai / thừa** |
| — | Code sửa của AI | Chạy thử nguyên đoạn code AI đưa 5 lần (`[Repeat(5)]`) | Pass 5/5 |

### Nhận xét

AI chẩn đoán đúng nguyên nhân chính: `FindElement` chạy ngay sau khi bấm Start trong khi `#finish` khoảng 5 giây sau mới được JavaScript thêm vào DOM, và AI cũng nhận ra từ con số 10/10 rằng đây là lỗi lần nào cũng xảy ra chứ không hẳn là flaky. Gợi ý chờ phần tử hiển thị và có chữ là đúng cho trang /1, còn với trang /2 thì không cần thiết vì phần tử vừa xuất hiện đã có chữ. Gợi ý "bấm Start trước khi trang tải xong" không liên quan, vì Selenium đã tự chờ trang tải và lỗi luôn nằm ở dòng tìm `#finish`. Lưu ý về `IgnoreExceptionTypes` là thừa, vì `WebDriverWait` mặc định đã bỏ qua `NotFoundException`, code của em không có dòng đó vẫn pass 10/10. Vì vậy em giữ cách sửa của mình ở Phần A và chỉ thêm kiểm tra `Displayed`, không tăng timeout khi chưa có bằng chứng server chậm.

---

# Lab 2 — Selenium WebDriver cơ bản: tương tác với trình duyệt và phần tử web

Trang kiểm thử: https://www.saucedemo.com — làm tiếp trên repo của Lab 1, file test: `FirstTests/LoginTests.cs`.

## Phần A — Bảng locator

Khảo sát bằng DevTools (chuột phải → Inspect), thứ tự ưu tiên: `id` → `data-test` → CSS Selector / XPath tương đối. Mọi locator đều được kiểm chứng bằng **Ctrl+F** trong tab Elements, kết quả **1 of 1**. Không dùng XPath tuyệt đối.

| # | Phần tử | Cách 1 | Cách 2 | Chọn | Vì sao | Ảnh Ctrl+F |
|---|---|---|---|---|---|---|
| 1 | Ô Username | `#user-name` (By.Id) | `[data-test="username"]` (CSS) | Cách 1 | `id` là duy nhất trên trang, ngắn, Selenium tìm theo id nhanh nhất | `lab2_A1_username_id.png`, `lab2_A1_username_datatest.png` |
| 2 | Ô Password | `#password` (By.Id) | `[data-test="password"]` (CSS) | Cách 1 | Giống ô Username: có `id` cố định, duy nhất | `lab2_A2_password_id.png`, `lab2_A2_password_datatest.png` |
| 3 | Nút Login | `#login-button` (By.Id) | `[data-test="login-button"]` (CSS) | Cách 1 | Có `id` riêng; không dùng `input[type=submit]` vì dễ trùng khi trang thêm form khác | `lab2_A3_login_id.png`, `lab2_A3_login_datatest.png` |
| 4 | Tên sản phẩm "Sauce Labs Backpack" | `#item_4_title_link [data-test="inventory-item-name"]` (CSS) | `//div[@data-test='inventory-item-name' and text()='Sauce Labs Backpack']` (XPath) | Cách 2 | Mô tả đúng danh tính "sản phẩm tên Backpack"; cách 1 phụ thuộc mã nội bộ `item_4` có thể đổi khi thêm/bớt sản phẩm | `lab2_A4_product_name_css.png`, `lab2_A4_product_name_xpath.png` |
| 5 | Giá sản phẩm "Sauce Labs Backpack" | `//div[@data-test='inventory-item'][.//div[@data-test='inventory-item-name' and text()='Sauce Labs Backpack']]//div[@data-test='inventory-item-price']` (XPath) | `[data-test="inventory-item"]:has(#item_4_title_link) [data-test="inventory-item-price"]` (CSS) | Cách 1 | Ô giá không có `id`, nên tìm khối sản phẩm theo tên rồi lấy giá bên trong; không phụ thuộc vị trí sản phẩm trong danh sách | `lab2_A5_product_price_xpath.png`, `lab2_A5_product_price_css.png` |

Locator dùng cho danh sách ở Phần C: `[data-test="inventory-item"]` khớp **6** phần tử (ảnh `lab2_C_inventory_items.png`), bên trong mỗi sản phẩm dùng `[data-test="inventory-item-name"]` và `[data-test="inventory-item-price"]`.

## Phần B — Kịch bản đăng nhập (thuận & nghịch)

File `FirstTests/LoginTests.cs`: `[SetUp]` khởi tạo ChromeDriver và mở trang đăng nhập, `[TearDown]` gọi `Quit()` rồi `Dispose()`. Các bước đăng nhập được gom vào hàm `Login(username, password)` để dùng lại.

| Ca | Dữ liệu | Assert |
|---|---|---|
| Ca 1 — luồng thuận | `standard_user` / `secret_sauce` | URL chuyển sang `https://www.saucedemo.com/inventory.html` |
| Ca 2 — luồng nghịch | `locked_out_user` / `secret_sauce` | Thông báo lỗi `[data-test="error"]` hiện ra, nội dung đúng là `Epic sadface: Sorry, this user has been locked out.`, vẫn ở trang đăng nhập |

Ảnh kết quả: `screenshots/lab2_B_login_tests_passed.png`.

## Phần C — Thu thập dữ liệu sản phẩm

Ca `Inventory_ShouldListSixProductsWithValidNameAndPrice`: đăng nhập, dùng `FindElements` lấy danh sách sản phẩm, rồi dùng `item.FindElement(...)` để lấy tên và giá **bên trong từng sản phẩm** và in ra. Các Assert:

- Có đúng 6 sản phẩm.
- Không có tên nào rỗng, không có tên trùng nhau.
- Danh sách có sản phẩm `Sauce Labs Backpack`, và giá của nó đúng là `$29.99`.
- Mọi giá đều bắt đầu bằng ký tự `$` và là số dương.

Ảnh kết quả: `screenshots/lab2_C_product_data_passed.png`, `screenshots/lab2_C_test_explorer_passed.png`.

Chạy test: mở `Lab1_KiemThuAI.sln` bằng Visual Studio → **Test → Test Explorer → Run All Tests**, hoặc:

```bash
dotnet test --filter FullyQualifiedName~LoginTests
```


## Phần D — Lồng ghép AI: tối ưu locator

**Phần tử chọn:** giá của sản phẩm "Sauce Labs Backpack" (ô giá không có `id`). Locator ban đầu là XPath tuyệt đối — mô tả đường đi từ `/html/body` xuống ô giá (chỉ để so sánh, không dùng trong code):

```
/html/body/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[2]/div[1]
```

### Prompt đã dùng

> Em đang viết test Selenium C# (NUnit) cho trang https://www.saucedemo.com, đã đăng nhập vào trang inventory.html. Em muốn lấy giá của sản phẩm "Sauce Labs Backpack". Hiện em dùng XPath copy từ DevTools:
> `/html/body/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[2]/div[1]`
>
> Đây là outerHTML của sản phẩm đó em copy từ DevTools:
>
> ```html
> <div class="inventory_item" data-test="inventory-item"><div class="inventory_item_img"><a href="#" id="item_4_img_link" data-test="item-4-img-link" role="button" aria-label="View details for Sauce Labs Backpack"><img alt="Sauce Labs Backpack" class="inventory_item_img" data-test="inventory-item-sauce-labs-backpack-img" src="/assets/sauce-backpack-1200x1500-CjRW-Djj.jpg"></a></div><div class="inventory_item_description" data-test="inventory-item-description"><div class="inventory_item_label"><a href="#" id="item_4_title_link" data-test="item-4-title-link" role="button" aria-label="View details for Sauce Labs Backpack"><div class="inventory_item_name " data-test="inventory-item-name">Sauce Labs Backpack</div></a><div class="inventory_item_desc" data-test="inventory-item-desc">carry.allTheThings() with the sleek, streamlined Sly Pack that melds uncompromising style with unequaled laptop and tablet protection.</div></div><div class="pricebar"><div class="inventory_item_price" data-test="inventory-item-price">$29.99</div><button class="btn btn_primary btn_small btn_inventory " data-test="add-to-cart-sauce-labs-backpack" id="add-to-cart-sauce-labs-backpack" name="add-to-cart-sauce-labs-backpack">Add to cart</button></div></div></div>
> ```
>
> Anh/chị đề xuất giúp em locator bền vững hơn để lấy đúng giá của sản phẩm này (CSS Selector hoặc XPath), và giải thích vì sao nó bền vững hơn XPath tuyệt đối ạ.

### Câu trả lời của AI (Claude)

> Em nên tìm giá theo **tên sản phẩm** kết hợp với thuộc tính **`data-test`**, đừng dựa vào vị trí trong cây DOM.
>
> **Cách 1: XPath (nên dùng)**
> ```xpath
> //div[@data-test='inventory-item'][.//div[@data-test='inventory-item-name' and normalize-space()='Sauce Labs Backpack']]//div[@data-test='inventory-item-price']
> ```
> Nghĩa là: tìm khối sản phẩm có tên "Sauce Labs Backpack", rồi lấy ô giá nằm trong khối đó. `normalize-space()` bỏ khoảng trắng thừa ở đầu và cuối chữ.
>
> **Cách 2: CSS Selector**
> CSS không lọc được theo chữ hiển thị. Vì vậy em dựa vào id của nút "Add to cart", id này có chứa tên sản phẩm:
> ```css
> .pricebar:has(#add-to-cart-sauce-labs-backpack) [data-test="inventory-item-price"]
> ```
> Selector này cần `:has()`, Chrome và Edge bản mới đều hỗ trợ. Lưu ý: khi đã thêm vào giỏ, nút này đổi thành `remove-sauce-labs-backpack`, nên selector sẽ không còn khớp. Cách 1 không bị ảnh hưởng.
>
> **Code C# (NUnit)**
> ```csharp
> var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
> By priceLocator = By.XPath(
>     "//div[@data-test='inventory-item']" +
>     "[.//div[@data-test='inventory-item-name' and normalize-space()='Sauce Labs Backpack']]" +
>     "//div[@data-test='inventory-item-price']");
>
> IWebElement priceEl = wait.Until(d => d.FindElement(priceLocator));
> Assert.That(priceEl.Text, Is.EqualTo("$29.99"));
> ```
>
> **Vì sao bền vững hơn XPath tuyệt đối?**
> - **XPath tuyệt đối phụ thuộc từng tầng `div[1]/div[2]...`.** Chỉ cần thêm một thẻ bọc, một banner, hoặc đổi bố cục là locator hỏng.
> - **Nó có thể lấy nhầm mà không báo lỗi.** Nếu em sắp xếp lại danh sách (ví dụ "Price (low to high)"), vị trí đó sẽ thuộc về sản phẩm khác. Test lấy sai giá mà không có lỗi gì báo ra.
> - **`data-test` được làm riêng cho việc kiểm thử.** Lập trình viên ít khi đổi nó khi chỉnh giao diện, khác với `class`. Ví dụ `class="inventory_item_name "` có một dấu cách thừa ở cuối, nếu so sánh `@class` chính xác sẽ dễ sai.
> - **Locator gắn với ý nghĩa nghiệp vụ** ("giá của sản phẩm tên X"), nên đọc code là hiểu ngay test đang kiểm tra gì.

### Tự kiểm chứng bằng Ctrl+F

| Locator | Trạng thái trang | Kết quả Ctrl+F | Ảnh |
|---|---|---|---|
| XPath tuyệt đối (ban đầu) | Mặc định | 1 of 1, đúng giá $29.99 | `lab2_D0_absolute_xpath.png` |
| XPath tuyệt đối (ban đầu) | Sắp xếp "Price (low to high)" | 1 of 1 nhưng trỏ sang **$7.99** của Sauce Labs Onesie — lấy nhầm mà không báo lỗi | `lab2_D5_absolute_xpath_after_sort.png` |
| AI cách 1 — XPath theo tên + `data-test` | Mặc định | 1 of 1, đúng $29.99 | `lab2_D1_ai_xpath.png` |
| AI cách 1 — XPath theo tên + `data-test` | Sau khi bấm "Add to cart" | Vẫn 1 of 1 | `lab2_D4_ai_xpath_after_add_to_cart.png` |
| AI cách 2 — CSS `:has(#add-to-cart-...)` | Mặc định | 1 of 1 | `lab2_D2_ai_css.png` |
| AI cách 2 — CSS `:has(#add-to-cart-...)` | Sau khi bấm "Add to cart" | **Không khớp** (nút đổi id thành `remove-sauce-labs-backpack`) | `lab2_D3_ai_css_after_add_to_cart.png` |

### Nhận xét

AI đề xuất đúng hướng: bỏ XPath tuyệt đối, tìm khối sản phẩm theo tên rồi lấy ô giá bên trong bằng `data-test`. Em dùng Ctrl+F kiểm tra thì XPath của AI khớp 1 of 1, và vẫn đúng sau khi bấm "Add to cart". Hai cảnh báo của AI cũng đúng: XPath tuyệt đối sẽ trỏ sang sản phẩm khác khi sắp xếp lại danh sách ($7.99 thay vì $29.99), còn CSS dựa vào id nút "Add to cart" thì không còn khớp khi nút đổi thành "Remove". Chỗ AI chưa đầy đủ: code mẫu thiếu dòng `using OpenQA.Selenium.Support.UI;` để dùng được `WebDriverWait`, và AI không nói cách 1 phụ thuộc vào chữ hiển thị, nếu đổi tên sản phẩm thì locator phải sửa theo. Vì vậy em chọn cách 1 của AI nhưng vẫn tự kiểm chứng lại bằng Ctrl+F trước khi dùng.

---

# Lab 1 — Nhập môn kiểm thử tự động & Thiết lập môi trường

Project kiểm thử tự động đầu tiên bằng **C# + NUnit + Selenium WebDriver**, làm trên **Visual Studio Community 2026**.

Repository: https://github.com/kiennctb01907/lab1_kiemthuAI

## Cấu trúc

```
lab1_kiemthuAI/
├── Lab1_KiemThuAI.sln              # Solution mở bằng Visual Studio
├── FirstTests/
│   ├── FirstTests.csproj           # Project NUnit + gói Selenium.WebDriver
│   ├── DriverFactory.cs            # Tạo ChromeDriver với ChromeOptions dùng chung
│   ├── FirstSeleniumTest.cs        # Test 1: selenium.dev
│   └── SecondSeleniumTest.cs       # Test 2: example.com
├── docs/
│   ├── HUONG_DAN_CAI_DAT.md        # Hướng dẫn cài môi trường (Phần A)
│   └── QUY_TRINH_GIT.md            # Quy trình Git/GitHub của nhóm (Phần C)
├── screenshots/                    # Ảnh minh chứng (xem screenshots/README.md)
├── .gitignore                      # Tạo bằng: dotnet new gitignore (bỏ qua bin/, obj/)
└── README.md
```

## Môi trường (Phần A)

| Thành phần | Phiên bản |
|---|---|
| Hệ điều hành | Windows 11 (10.0.26200) |
| .NET SDK | 10.0.301 |
| IDE | Visual Studio Community 2026 |
| Test framework | NUnit 4.3.2, NUnit3TestAdapter 5.0.0 |
| Selenium.WebDriver | 4.52.0 |
| Trình duyệt | Google Chrome 156 (ChromeDriver do Selenium Manager tự tải) |

Ảnh: `screenshots/01_dotnet_version.png`, `02_dotnet_info.png`, `03_visual_studio_csproj.png`.

## Kịch bản test (Phần B)

**Test 1** — `FirstSeleniumTest.OpenSeleniumHomePage_TitleShouldContainSelenium`:

1. `[SetUp]` khởi tạo ChromeDriver (qua `DriverFactory`) → mở Chrome.
2. `driver.Navigate().GoToUrl("https://www.selenium.dev/")`.
3. Đọc `driver.Title` và in ra màn hình (`Console.WriteLine`) → `Tiêu đề trang: Selenium`.
4. `Assert.That(title, Does.Contain("Selenium"))` và kiểm tra URL.
5. `[TearDown]` gọi `driver.Quit()` để đóng trình duyệt.

**Test 2** — `SecondSeleniumTest.OpenExampleDomain_TitleAndContentShouldMatch`: vào https://example.com/, kiểm tra tiêu đề đúng là `Example Domain` và đoạn văn đầu tiên.

Kết quả: **2/2 Passed** — ảnh `screenshots/04_visual_studio_test_run.png`, `05_test_explorer_passed.png`, `06_dotnet_test_passed.png`.

## Cách chạy

### Trên Visual Studio
1. Mở file `Lab1_KiemThuAI.sln` (**File → Open → Project/Solution**).
2. **Build → Build Solution** (`Ctrl+Shift+B`). Lần đầu VS sẽ tự restore gói NuGet.
3. **Test → Test Explorer** (`Ctrl+E, T`) → bấm **Run All Tests**.
4. Chrome tự bật, vào trang web rồi tự đóng; Test Explorer hiện dấu ✔ **Passed**.
5. Chọn một test → khung bên phải → **Open additional output for this result** để xem dòng `Tiêu đề trang: ...`.

### Bằng dòng lệnh
```bash
dotnet test
```

Chạy không mở cửa sổ Chrome (headless), trong PowerShell:
```bash
$env:HEADLESS='1'; dotnet test
```

## Quản lý mã nguồn (Phần C)

- Repository public, có `.gitignore` cho .NET → không commit `bin/`, `obj/`.
- Mỗi thành viên làm trên một branch riêng, tạo Pull Request vào `main`, nhóm trưởng review rồi merge. Ảnh lịch sử commit: `screenshots/07_commit_history_github.jpg`.
- Chi tiết quy trình: [docs/QUY_TRINH_GIT.md](docs/QUY_TRINH_GIT.md).

## Phần D — Lồng ghép AI

Nhóm dùng trợ lý AI (Claude Code) trong quá trình cài đặt, viết code và đưa lên GitHub. Các tình huống thực tế:

### Tình huống 1: Test thứ 2 bị lỗi `NoSuchElementException`

Lần chạy đầu tiên test trên example.com bị **Failed**:

```
OpenQA.Selenium.NoSuchElementException : no such element: Unable to locate element: {"method":"tag name","selector":"h1"}
  (Session info: chrome=156.0.8078.80)
```

**Prompt:** "Test Selenium C# (NUnit, Windows 11) mở https://example.com/ rồi `driver.FindElement(By.TagName("h1"))` bị `NoSuchElementException` như trên. Vì sao và sửa thế nào?"

**Câu trả lời của AI (tóm tắt):** Lỗi nghĩa là trên trang không có phần tử khớp locator. AI đề xuất mở trang và kiểm tra cấu trúc HTML (DevTools F12). Kết quả là example.com hiện **không còn thẻ `<h1>`**, nội dung nằm trong các thẻ `<p>`. Cách sửa: đổi locator sang `By.TagName("p")` và kiểm tra đoạn văn có chứa `documentation examples`. Sau khi sửa, test **Passed**.

### Tình huống 2: Không push được lên GitHub từ terminal

```
fatal: Cannot prompt because user interactivity has been disabled.
fatal: could not read Username for 'https://github.com': terminal prompts disabled
```

**Prompt:** "Chạy `git push -u origin main` trên Windows bị lỗi trên. Repo là HTTPS trên GitHub. Sửa thế nào?"

**Câu trả lời của AI (tóm tắt):** Git chưa có thông tin đăng nhập GitHub, và terminal đang chạy ở chế độ không cho hiện hộp thoại đăng nhập. Cách xử lý: chạy lại `git push` trong một terminal tương tác (hoặc dùng nút **Push** trong Visual Studio), để Git Credential Manager mở cửa sổ đăng nhập GitHub. Sau lần đăng nhập đầu, thông tin được lưu lại. Làm theo thì push thành công.

### Tình huống 3: Lệnh `gh` không được nhận diện

```
gh : The term 'gh' is not recognized as the name of a cmdlet, function, script file, or operable program.
```

**Câu trả lời của AI (tóm tắt):** Máy chưa cài GitHub CLI. Có thể cài bằng `winget install --id GitHub.cli`. Tuy nhiên `gh` không bắt buộc: có thể tạo repository và Pull Request ngay trên github.com. Nhóm chọn cách làm trên giao diện web.

### Nhận xét

AI hữu ích nhất ở chỗ đọc thông báo lỗi và chỉ ra nhanh nguyên nhân gốc: thiếu đăng nhập Git, thiếu công cụ `gh`, locator sai do trang web đã đổi. Nhờ vậy nhóm không mất thời gian thử mò. AI cũng đưa ra lệnh và đoạn code sửa cụ thể, giải thích được vì sao sửa như vậy. Tuy nhiên AI không tự biết trang web đã thay đổi: đoạn code mẫu ban đầu vẫn dùng thẻ `<h1>` theo giao diện cũ của example.com, và chỉ sửa đúng khi có thông báo lỗi thật. Vì vậy nhóm luôn chạy lại `dotnet test` hoặc lệnh kiểm tra để tự xác nhận, thay vì tin ngay vào gợi ý của AI.
