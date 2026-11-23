**[Vai trò]** Bạn là một kỹ sư kiểm thử tự động (QA automation) có kinh nghiệm với Selenium WebDriver C# và NUnit, đang giúp một nhóm sinh viên lên bộ test case.

**[Ngữ cảnh framework]** Nhóm kiểm thử trang https://www.saucedemo.com (tài khoản standard_user / secret_sauce). Framework viết bằng C# .NET 10, NUnit 4, Selenium 4.52, theo mô hình Page Object Model: mọi class test kế thừa `BaseTest` (tự mở trình duyệt, mở trang đăng nhập, đóng trình duyệt, ghi log OK/RỚT, chụp ảnh khi rớt); mọi Page kế thừa `BasePage` (có hàm `Tim(By)` chờ phần tử, locator để `private readonly By`). Đã có sẵn `LoginPage`, `InventoryPage` (danh sách 6 sản phẩm, sắp xếp) và `CheckoutPage` (trang Your Information). Dữ liệu cấu hình nằm trong `appsettings.json`, test chạy song song theo class.

**[Yêu cầu]** Tính năng cần kiểm thử: **thêm sản phẩm vào giỏ hàng và thanh toán** — từ trang danh sách sản phẩm bấm "Add to cart", xem giỏ hàng (trang cart.html), xóa sản phẩm khỏi giỏ, bấm Checkout, điền thông tin, xem trang tổng tiền (Overview) và bấm Finish để hoàn tất đơn. Hãy liệt kê bộ test case cho tính năng này, gồm đủ 3 loại:
- ca thường (luồng đúng),
- ca biên (số lượng sản phẩm, giá trị ở ranh giới),
- ca lỗi (thao tác sai, thiếu dữ liệu).

Khoảng 12–15 ca là đủ. Chỉ liệt kê test case, chưa cần viết code.

**[Định dạng đầu ra]** Trình bày đúng một bảng Markdown với các cột: `Mã` (TC01, TC02…) | `Loại` (Thường / Biên / Lỗi) | `Tên ca` | `Các bước` | `Kết quả mong đợi`. Không viết thêm phần giải thích nào ngoài bảng.
