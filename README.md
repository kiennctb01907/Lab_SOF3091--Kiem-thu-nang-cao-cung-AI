# Lab 1 — Nhập môn kiểm thử tự động & Thiết lập môi trường

Project kiểm thử tự động đầu tiên bằng **C# + NUnit + Selenium WebDriver**, làm trên **Visual Studio Community 2026**.

## Cấu trúc

```
lab1_kiemthuAI/
├── Lab1_KiemThuAI.sln          # Solution mở bằng Visual Studio
├── FirstTests/
│   ├── FirstTests.csproj       # Project NUnit + gói Selenium.WebDriver
│   └── FirstSeleniumTest.cs    # Kịch bản Selenium đầu tiên
├── screenshots/                # Ảnh minh chứng (môi trường, kết quả test, lịch sử commit)
├── .gitignore                  # Tạo bằng: dotnet new gitignore (bỏ qua bin/, obj/)
└── README.md
```

## Môi trường

| Thành phần | Phiên bản |
|---|---|
| Hệ điều hành | Windows 11 |
| .NET SDK | 10.0.301 |
| IDE | Visual Studio Community 2026 |
| Test framework | NUnit 4.3.2, NUnit3TestAdapter 5.0.0 |
| Selenium.WebDriver | 4.52.0 |
| Trình duyệt | Google Chrome (ChromeDriver được Selenium Manager tự tải) |

## Kịch bản test (Phần B)

`FirstSeleniumTest.OpenSeleniumHomePage_TitleShouldContainSelenium`:

1. `[SetUp]` khởi tạo `ChromeDriver` → mở Chrome.
2. `driver.Navigate().GoToUrl("https://www.selenium.dev/")`.
3. Đọc `driver.Title` và in ra (`Console.WriteLine`).
4. `Assert.That(title, Does.Contain("Selenium"))` và kiểm tra URL.
5. `[TearDown]` gọi `driver.Quit()` để đóng trình duyệt.

## Cách chạy

### Trên Visual Studio
1. Mở file `Lab1_KiemThuAI.sln` (File → Open → Project/Solution).
2. Build → Build Solution (`Ctrl+Shift+B`). Lần đầu VS sẽ tự restore gói NuGet.
3. Test → Test Explorer (`Ctrl+E, T`) → bấm **Run All Tests**.
4. Chrome tự bật, vào trang selenium.dev rồi tự đóng; Test Explorer hiện dấu ✔ **Passed**.
5. Chọn test trong Test Explorer → khung bên phải → **Open additional output for this result** để xem dòng `Tiêu đề trang: Selenium`.

### Bằng dòng lệnh
```bash
dotnet test
```

Kết quả mong đợi:
```
Passed OpenSeleniumHomePage_TitleShouldContainSelenium
Total tests: 1
     Passed: 1
```

## Quy trình làm việc nhóm trên GitHub

- Nhóm trưởng tạo repository, đẩy khung project lên branch `main`.
- Mỗi thành viên làm trên branch riêng, ví dụ `feature/first-selenium-test`.
- Thành viên push branch → tạo **Pull Request** vào `main` → nhóm trưởng review → **Merge pull request**.

## Phần D — Lồng ghép AI

### Tình huống 1: Lỗi `gh` không được nhận diện

Khi kiểm tra công cụ để làm việc với GitHub, chạy `gh --version` trong PowerShell thì báo lỗi:

```
gh : The term 'gh' is not recognized as the name of a cmdlet, function, script file, or operable program.
```

**Prompt đã dùng:** "Tôi dùng Windows 11, PowerShell, chạy `gh --version` bị lỗi *The term 'gh' is not recognized...*. Lỗi này là gì và sửa thế nào?"

**Câu trả lời của AI (tóm tắt):** Máy chưa cài GitHub CLI (`gh`), hoặc đã cài nhưng chưa có trong biến môi trường `PATH`. Có thể cài bằng `winget install --id GitHub.cli`, sau đó đóng và mở lại terminal. Ngoài ra `gh` không bắt buộc: có thể tạo repository và Pull Request trực tiếp trên trang github.com, còn push code thì dùng `git` hoặc cửa sổ **Git Changes** của Visual Studio.

### Tình huống 2: Git chưa có danh tính người commit

Trước khi commit lần đầu, `git config user.name` trả về rỗng; nếu commit luôn Git sẽ báo `Author identity unknown`. AI gợi ý đặt tên/email bằng `git config user.name "..."` và `git config user.email "..."` (hoặc trong Visual Studio: Git → Settings → Git Global Settings), dùng đúng email tài khoản GitHub để commit hiện đúng người trên GitHub.

### Nhận xét

<!-- TODO: Sinh viên tự viết lại phần này bằng lời của mình (3–5 câu) -->
AI giúp nhận ra nhanh nguyên nhân lỗi (thiếu công cụ / thiếu cấu hình) và đưa ra lệnh sửa cụ thể, tiết kiệm thời gian tra cứu. AI cũng giải thích được lựa chọn thay thế (dùng giao diện web GitHub hoặc Visual Studio thay cho `gh`). Tuy nhiên AI không biết chính xác trạng thái máy của mình, nên một số gợi ý (như lệnh cài đặt hay đường dẫn) cần tự kiểm chứng lại trước khi chạy. Mình luôn chạy lại lệnh kiểm tra (`--version`, `dotnet test`) để xác nhận lỗi đã được khắc phục thật sự.
