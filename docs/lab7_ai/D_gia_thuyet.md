# Phần D - Giả thuyết tự đoán (viết TRƯỚC khi hỏi AI)

**Lỗi gặp:** chạy `GioHangTests`, 10/12 ca xanh, 2 ca rớt là `XoaMotSanPhamTrongGio_ConLaiSanPhamKia` và `XoaSanPhamCuoiCung_GioTrong_HuyHieuMat`.

```
OpenQA.Selenium.WebDriverTimeoutException : Timed out after 10 seconds
  at OpenQA.Selenium.Support.UI.DefaultWait`1.ThrowTimeoutException(String exceptionMessage, Exception lastException)
  at OpenQA.Selenium.Support.UI.DefaultWait`1.Until[TResult](Func`2 condition)
  at FirstTests.Pages.CartPage.XoaSanPham(String ten) in ...\Pages\CartPage.cs:line 71
  at FirstTests.Tests.GioHangTests.XoaSanPhamCuoiCung_GioTrong_HuyHieuMat() in ...\Tests\GioHangTests.cs:line 138
```

**Đọc stack trace:**
- Dòng trên cùng thuộc code của nhóm là `CartPage.cs:71`, tức `wait.Until(_ => DaBiGoKhoiTrang(dong))` trong `XoaSanPham`.
- Lỗi là timeout chứ không phải `NoSuchElementException`. Vậy `TimDongTheoTen` và cú click nút Remove vẫn chạy được. Chỉ có điều kiện chờ không bao giờ trả về true trong 10 giây.
- Cả hai ca rớt đều gọi `XoaSanPham`. Mười ca không gọi hàm này đều xanh.

**Giả thuyết của mình:** điều kiện chờ sai, không phải locator sai. `DaBiGoKhoiTrang` chỉ trả true khi phần tử cũ ném `StaleElementReferenceException`, tức là dòng phải bị gỡ hẳn khỏi DOM. Mình đoán saucedemo (React) không gỡ thẻ `div` của dòng đó. Trang chỉ đổi nội dung hoặc class, nên phần tử cũ vẫn "sống" và không bao giờ stale. Cách kiểm: sau khi bấm Remove, in `outerHTML` của `.cart_list` và thử đọc `Displayed` của phần tử cũ.

**Kiểm chứng (test thăm dò tạm, đã xóa sau khi xem):**
- Sau khi bấm Remove, `.cart_list` còn một thẻ rỗng `<div class="removed_cart_item"></div>` nằm đúng chỗ dòng cũ.
- `.cart_item` giảm từ 2 xuống 1.
- Phần tử cũ trả `Displayed=False` và không ném lỗi stale.

Kết luận: đúng như đoán. Trang tái dùng thẻ cũ nên chờ "stale" sẽ chờ mãi. Mình định sửa thành chờ số dòng `.cart_item` giảm đi 1. Cách này không phụ thuộc việc trang gỡ thẻ hay chỉ đổi class.
