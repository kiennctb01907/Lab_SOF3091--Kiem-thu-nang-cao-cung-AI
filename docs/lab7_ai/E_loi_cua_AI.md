# Phần E - Những chỗ AI sai trong Lab 7

Trong lab này nhóm bắt được 3 chỗ AI sai, 2 chỗ nặng và 1 chỗ nhẹ. Cả ba đều nằm trong câu trả lời đã lưu nguyên văn (`A_tra_loi.md`, `C_tra_loi.md`).

## Lỗi 1 - Kỳ vọng sai nghiệp vụ: TC15 "giỏ trống phải bị chặn"

- **Prompt:** `A_prompt.md` (nhờ AI liệt kê test case thêm vào giỏ và thanh toán).
- **AI trả lời (TC15):** "Đăng nhập, không thêm sản phẩm nào, vào giỏ, bấm Checkout. Kết quả mong đợi: hệ thống phải chặn hoặc báo giỏ trống, không cho tạo đơn rỗng (nếu vẫn cho qua tới Finish thì ghi nhận là lỗi)."
- **Trên trang thật:** giỏ trống vẫn bấm được Checkout và điền thông tin được. Trang Overview hiện `Item total: $0`, `Tax: $0.00`, `Total: $0.00`, bấm Finish vẫn ra "Thank you for your order!".
- **Vì sao sai:** AI suy theo "một web bán hàng bình thường thì nên như vậy" rồi viết thành kỳ vọng chắc chắn. Saucedemo không có quy tắc đó. Nếu viết test theo TC15 thì test sẽ đỏ vĩnh viễn dù trang chạy đúng thiết kế, và người đọc báo cáo sẽ tưởng trang có bug. Muốn báo đây là lỗi thì phải có yêu cầu nghiệp vụ ghi rõ, chứ không lấy suy đoán của AI làm chuẩn.
- **Cách phát hiện:** chạy test thăm dò đi đúng các bước của TC15 rồi in URL và số tiền ở mỗi bước (Phần B).
- **Xử lý:** BỎ TC15.

## Lỗi 2 - Điều kiện chờ sai trong CartPage do AI sinh (gây timeout thật)

- **Prompt:** `C_prompt.md` (nhờ AI viết `Pages/CartPage.cs` theo quy ước của nhóm).
- **AI trả lời:** `XoaSanPham` bấm Remove rồi `wait.Until(_ => DaBiGoKhoiTrang(dong))`. Hàm này chỉ trả true khi dòng cũ ném `StaleElementReferenceException`. Phần ghi chú của AI còn khẳng định: "XoaSanPham chờ dòng bị gỡ khỏi DOM (stale) thay vì dùng Thread.Sleep".
- **Trên trang thật:** bấm Remove xong, React dùng lại đúng thẻ `div` đó và chỉ đổi class thành `removed_cart_item` rỗng. Phần tử cũ vẫn sống (`Displayed=False`) và không bao giờ stale, nên lần nào cũng chờ hết 10 giây rồi `WebDriverTimeoutException`.
- **Vì sao sai:** AI đúng ở chỗ "không dùng Thread.Sleep, phải chờ có điều kiện". Nhưng điều kiện nó chọn dựa trên giả định về cách trang cập nhật DOM mà không kiểm. Code vẫn build sạch, locator đều đúng, đọc qua thấy rất hợp lý. Chỉ khi chạy mới lộ ra.
- **Cách phát hiện:** chạy `GioHangTests` thì 2 ca có gọi `XoaSanPham` rớt timeout. Stack trace chỉ thẳng `CartPage.cs:71`. Nhóm tự đoán nguyên nhân trước (`D_gia_thuyet.md`), in `outerHTML` để kiểm, rồi mới hỏi một phiên AI mới (Phần D).
- **Xử lý:** đổi thành chờ "không còn dòng `.cart_item` nào mang tên này", mỗi vòng chờ tìm lại từ đầu.

## Lỗi 3 (nhẹ) - Bịa ra "giới hạn" không có: TC07 "số lượng tối đa"

- AI gọi ca thêm cả 6 sản phẩm là ca biên "số lượng tối đa". Saucedemo không giới hạn số lượng. 6 chỉ là toàn bộ sản phẩm đang bán, và mỗi sản phẩm chỉ thêm được 1 lần (nút đổi sang Remove). Số tiền AI tính cho ca này thì đúng.
- **Xử lý:** SỬA. Giữ ý "thêm hết sản phẩm", bỏ chữ "tối đa", và gộp vào ca kiểm tổng tiền bằng phép cộng giá đọc từ giỏ.

## Những gì AI làm đúng (đã kiểm từng tiêu chí)

| Tiêu chí | Kết quả kiểm |
|---|---|
| Locator thật | 10/10 locator trong `CartPage` khớp đúng số phần tử trên `cart.html` (test `KiemLocatorCartPageTests`) |
| Số liệu trong test case | Giá, thuế, tổng của TC04, TC05, TC07, TC10 đúng tới từng xu. Các câu báo lỗi TC12 đến TC14 đúng nguyên văn |
| Thread.Sleep | Không có trong `CartPage` (Find in Files ra 0 kết quả trong `Pages/`) |
| API lỗi thời | Không thấy. `FindElements`, `WebDriverWait.Until` đều đúng với Selenium 4.52 |
| Chẩn đoán ở Phần D | Đúng nguyên nhân, trùng với giả thuyết nhóm tự đoán trước. Cách sửa chạy xanh 12/12 |

Riêng câu trả lời Phần D có một gợi ý thừa: AI dặn `LaySoTrenHuyHieu()` "phải dùng FindElements, không dùng Tim()". Nhóm không làm theo, vì hàm đó đã hỏi `CoHienThi` trước rồi mới gọi `Tim`. AI không thấy đoạn code này nên dặn phòng xa.
