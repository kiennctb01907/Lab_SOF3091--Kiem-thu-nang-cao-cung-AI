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
