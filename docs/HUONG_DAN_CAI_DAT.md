# Hướng dẫn cài đặt & kiểm tra môi trường (Phần A)

> Người phụ trách: **Gia Huy**

## 1. Cài .NET SDK

**Windows** (chọn một trong các cách):

- Tải file cài đặt từ trang chính thức: https://dotnet.microsoft.com/download → chọn **.NET SDK** (không phải Runtime).
- Dùng Chocolatey: `choco install dotnet-sdk -y`
- Dùng winget: `winget install Microsoft.DotNet.SDK.10`

**macOS:** `brew install --cask dotnet-sdk`

Kiểm tra (mở **terminal mới** sau khi cài):

```bash
dotnet --version
```

```bash
dotnet --info
```

Nếu báo `command not found` / `'dotnet' is not recognized` → đóng hẳn terminal (hoặc Visual Studio) và mở lại để nhận biến môi trường `PATH` mới.

## 2. Cài IDE

### Visual Studio (Windows — nhóm dùng bản Community 2026)
1. Tải **Visual Studio Community** (miễn phí) tại https://visualstudio.microsoft.com/.
2. Trong Visual Studio Installer chọn workload **.NET desktop development** (có sẵn công cụ test và NuGet).
3. Sau khi cài: **Help → About Microsoft Visual Studio** để xem phiên bản.

### VS Code (macOS hoặc máy cấu hình yếu)
1. Cài VS Code tại https://code.visualstudio.com/.
2. Cài extension **C# Dev Kit** (tự kèm extension C#).

## 3. Tạo project NUnit và thêm Selenium

### Cách 1 — Trong Visual Studio
1. **File → New → Project** → tìm **NUnit Test Project** → đặt tên `FirstTests`.
2. Chuột phải project → **Manage NuGet Packages…** → tab **Browse** → tìm `Selenium.WebDriver` → **Install**.
3. Mở file `.csproj` (double-click tên project) để kiểm tra có dòng `PackageReference Include="Selenium.WebDriver"`.

### Cách 2 — Dòng lệnh
```bash
dotnet new nunit -n FirstTests
```

```bash
cd FirstTests
```

```bash
dotnet add package Selenium.WebDriver
```

## 4. ChromeDriver

- Từ Selenium 4.6 trở lên, **Selenium Manager** tự tải ChromeDriver khớp với phiên bản Chrome đang cài → không cần cài thủ công hay thêm gói `Selenium.WebDriver.ChromeDriver`.
- Chỉ cần máy đã cài **Google Chrome**.
- Lần chạy test đầu tiên sẽ chậm hơn vì Selenium Manager đang tải driver (lưu ở `%USERPROFILE%\.cache\selenium`).

## 5. Lỗi thường gặp

| Lỗi | Nguyên nhân | Cách xử lý |
|---|---|---|
| `'dotnet' is not recognized` | Terminal mở trước khi cài SDK | Mở lại terminal / Visual Studio |
| Test Explorer không thấy test | Chưa build hoặc thiếu `NUnit3TestAdapter` | Build lại (`Ctrl+Shift+B`), kiểm tra gói trong `.csproj` |
| `SessionNotCreatedException` | Phiên bản Chrome và ChromeDriver lệch nhau | Xóa gói ChromeDriver cũ (nếu có) để Selenium Manager tự tải bản đúng, cập nhật Chrome |
| `NoSuchElementException` | Không tìm thấy phần tử trên trang | Kiểm tra lại locator bằng DevTools (F12), trang có thể đã đổi giao diện |
