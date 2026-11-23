# Phần B - Thẩm định 16 ca AI đề xuất

Cách làm: mình viết một test thăm dò tạm. Test đi đúng các bước AI ghi cho từng ca trên saucedemo thật, in ra URL, số dòng trong giỏ, huy hiệu, chữ trên nút, số tiền ở trang Overview và câu báo lỗi. Sau đó mình đặt kết quả cạnh "Kết quả mong đợi" của AI để so. File thăm dò đã xóa sau khi lấy số liệu.

| Mã | AI mong đợi (tóm tắt) | Trang thật | Quyết định | Ghi chú |
|---|---|---|---|---|
| TC01 | Nút đổi "Remove", huy hiệu 1 | nút Remove, badge 1 | **GIỮ** | |
| TC02 | cart.html, 2 dòng, $29.99 và $9.99, qty 1 | đúng cả | **GIỮ** | |
| TC03 | Xóa Bike Light, còn 1 dòng, badge 1 | còn 1 dòng, badge 1 | **GIỮ** | |
| TC04 | 29.99 / 2.40 / 32.39, "Thank you for your order!", giỏ trống | đúng cả | **GIỮ** | |
| TC05 | 3 sp: 53.97 / 4.32 / 58.29 | đúng | **SỬA** | Số đúng nhưng AI tự tính sẵn. Nếu web đổi giá, test sẽ rớt mà không phải do lỗi tính tiền. Sửa thành cộng giá đọc từ giỏ rồi so với Item total, kiểm Tax = 8% và Total = Item + Tax |
| TC06 | Về Inventory, giỏ giữ sp cũ, thêm sp 2 thì giỏ có 2 | đúng | **GIỮ** | |
| TC07 | 6 sp "số lượng tối đa", 129.94 / 10.40 / 140.34 | badge 6, số tiền đúng | **SỬA** | Saucedemo không có giới hạn số lượng. 6 chỉ là toàn bộ sản phẩm đang bán, không phải biên. Bỏ chữ "tối đa", gộp vào ca tính tổng như TC05 |
| TC08 | Giỏ trống, huy hiệu biến mất | 0 dòng, không có badge | **GIỮ** | |
| TC09 | Nút đổi lại "Add to cart", huy hiệu mất | đúng | **GIỮ** | |
| TC10 | Onesie: 7.99 / 0.64 / 8.63 | đúng | **SỬA** | Gộp vào ca tính tổng (trường hợp 1 sản phẩm rẻ nhất) |
| TC11 | Tên 1 ký tự vẫn sang Overview | sang Overview | **BỎ** | Trùng ý với `MaBuuChinh_MotKyTu_SangBuocHai` trong CheckoutTests (Lab 6), không thêm giá trị kiểm thử cho giỏ hàng |
| TC12 | Trống First Name, "Error: First Name is required" | đúng | **BỎ** | Lab 6 đã có ca kiểm thông báo lỗi ở trang Your Information. Giữ TC13 làm đại diện là đủ |
| TC13 | Trống Last Name, "Error: Last Name is required" | đúng | **GIỮ** | Đi từ giỏ hàng sang, khác Lab 6 vốn mở thẳng trang checkout |
| TC14 | Trống Postal Code, "Error: Postal Code is required" | đúng | **BỎ** | Trùng hẳn `MaBuuChinh_BoTrong_BaoLoiBatBuoc` của Lab 6 |
| TC15 | Giỏ trống bấm Checkout thì **phải bị chặn** | Không chặn: sang Overview với Item total $0, Tax $0.00, Total $0.00, bấm Finish vẫn ra "Thank you for your order!" | **BỎ** | Kỳ vọng sai. AI tự thêm một quy tắc mà saucedemo không có. Viết theo kỳ vọng này thì test luôn đỏ dù trang chạy đúng thiết kế của nó. Ghi vào Phần E |
| TC16 | Cancel ở Overview về Inventory, giỏ vẫn 1 | đúng | **GIỮ** | |

**Tổng kết:** giữ 9 ca, sửa 3 ca (gộp thành 1 test 3 bộ dữ liệu), bỏ 4 ca. Lý do bỏ:
1. **TC15**: sai nghiệp vụ. AI suy theo "web bán hàng thông thường", còn saucedemo cho thanh toán giỏ rỗng.
2. **TC14**: trùng hoàn toàn ca bỏ trống Postal Code đã viết ở Lab 6.
3. **TC12**: cùng cơ chế báo lỗi bắt buộc nhập của trang Your Information, Lab 6 đã phủ. Giữ một ca TC13 đi từ giỏ hàng là đủ.
4. **TC11**: kiểm độ dài ô nhập, thuộc trang Your Information chứ không thuộc giỏ hàng, Lab 6 đã có ca biên 1 ký tự.
