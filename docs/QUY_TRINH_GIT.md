# Quy trình làm việc nhóm với Git/GitHub (Phần C)

> Người phụ trách: **Phương Nam**

Repository: https://github.com/kiennctb01907/lab1_kiemthuAI (public)

## 1. Vai trò

- **Nhóm trưởng (Kiên):** tạo repository, đẩy khung project lên `main`, review và merge Pull Request.
- **Thành viên:** mỗi người làm trên một branch riêng `feature/<tên>-<nội-dung>`, không commit thẳng vào `main`.

## 2. Các bước cho mỗi thành viên

### Trong Visual Studio
1. **Git → Clone Repository** → dán link repo → **Clone**.
2. Góc dưới bên phải bấm tên branch → **New Branch** → đặt tên, ví dụ `feature/employees`, base là `main`.
3. Code → mở cửa sổ **Git Changes** (`Ctrl+0, Ctrl+G`) → ghi message → **Commit All**.
4. Bấm **Push** (mũi tên lên) để đẩy branch lên GitHub.
5. Bấm link **Create a Pull Request** hiện ra sau khi push (hoặc vào GitHub → **Compare & pull request**).
6. Nhóm trưởng review tab **Files changed** → **Merge pull request** → **Confirm merge**.
7. Mọi người chuyển về `main` → **Git → Pull** để lấy code mới nhất trước khi tạo branch tiếp theo.

### Bằng dòng lệnh
```bash
git clone https://github.com/kiennctb01907/lab1_kiemthuAI.git
```

```bash
git switch -c feature/employees
```

```bash
git add .
```

```bash
git commit -m "Them chuc nang ..."
```

```bash
git push -u origin feature/employees
```

Sau khi PR được merge:

```bash
git switch main
```

```bash
git pull
```

## 3. Quy ước commit message

- Viết ngắn gọn, bắt đầu bằng động từ, mô tả đúng việc đã làm: `Them test Selenium ...`, `Sua locator ...`, `Cap nhat README ...`.
- Mỗi commit chỉ làm một việc.
- Không dùng message chung chung như `update`, `fix`, `abc`.

## 4. Những thứ KHÔNG được commit

- Thư mục `bin/`, `obj/`, `.vs/`, `TestResults/` (đã có trong `.gitignore`, tạo bằng `dotnet new gitignore`).
- Mật khẩu, token, khóa API.

Kiểm tra trước khi commit: `git status` không được có file trong `bin/` hoặc `obj/`.

## 5. Branch của nhóm

| Thành viên | Branch | Nội dung |
|---|---|---|
| Kiên (nhóm trưởng) | `feature/first-selenium-test`, `feature/kien-evidence-readme` | Khung project, test Selenium đầu tiên, README, ảnh minh chứng, phần D |
| Minh Phương | `feature/minh-phuong-second-test` | Test Selenium thứ 2 (example.com) |
| Vĩnh Phát | `feature/vinh-phat-chrome-options` | `DriverFactory` cấu hình ChromeOptions dùng chung |
| Gia Huy | `feature/gia-huy-setup-guide` | Tài liệu cài đặt môi trường |
| Phương Nam | `feature/phuong-nam-git-workflow` | Tài liệu quy trình Git nhóm |
