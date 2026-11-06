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
