## (1) Nguyên nhân gốc

Code của bạn chờ cho tới khi phần tử `.cart_item` cũ bị **stale** (`StaleElementReferenceException`). Trên saucedemo thì điều này không xảy ra.

Saucedemo viết bằng React. Khi bấm **Remove** ở trang giỏ hàng, component của dòng sản phẩm không bị gỡ hẳn khỏi cây. Nó render ra một thẻ rỗng thay cho dòng cũ:

```html
<!-- trước khi bấm -->
<div class="cart_item"> ... tên, giá, nút Remove ... </div>
<!-- sau khi bấm -->
<div class="removed_cart_item"></div>
```

Hai thẻ cùng là `div` và nằm ở cùng vị trí, nên React **dùng lại đúng node DOM cũ**. Nó chỉ đổi `className` và xoá các con bên trong. Vì vậy biến `dong` mà bạn giữ vẫn trỏ tới một node còn sống trong document:

- `phanTu.Displayed` chạy bình thường và trả về `false` (div rỗng), không ném `StaleElementReferenceException`.
- `DaBiGoKhoiTrang` luôn trả `false`, nên `wait.Until` chờ hết 10 giây rồi ném `WebDriverTimeoutException` ở dòng 71.

Việc click vẫn thành công: sản phẩm đã bị xoá thật, chỉ có điều kiện chờ là không bao giờ đúng. Vì vậy chỉ những test gọi `XoaSanPham` bị rớt, còn các test khác dùng `CartPage` vẫn xanh.

Bạn có thể tự kiểm chứng: mở DevTools ở trang `/cart.html`, chọn phần tử `.cart_item`, bấm Remove. Bạn sẽ thấy chính node đó đổi thành `div.removed_cart_item` chứ không biến mất.

Tóm lại, lỗi nằm ở điều kiện chờ: "phần tử cũ phải stale" là giả định về cách trang cập nhật DOM, và React không làm như vậy. Đây không phải lỗi thời gian chờ ngắn hay locator sai.

## (2) Code `XoaSanPham` đã sửa

Cách sửa là chờ theo **trạng thái quan sát được** thay vì theo vòng đời của một tham chiếu phần tử: mỗi vòng poll, tìm lại từ đầu xem còn dòng nào mang tên đó không.

```csharp
public CartPage XoaSanPham(string ten)
{
    TimDongTheoTen(ten).FindElement(nutXoa).Click();

    // Mỗi lần poll đều tìm lại từ đầu, không giữ tham chiếu cũ
    wait.Until(_ => !ConSanPhamTrongGio(ten));
    return this;
}

private bool ConSanPhamTrongGio(string ten)
{
    try
    {
        return driver.FindElements(dongSanPham)
                     .Any(dong => dong.FindElement(tenSanPham).Text.Trim() == ten);
    }
    catch (StaleElementReferenceException)
    {
        return true;   // DOM đang cập nhật giữa chừng -> coi như chưa xong, vòng sau kiểm tra lại
    }
    catch (NoSuchElementException)
    {
        return true;   // dòng đang bị đổi nội dung -> thử lại vòng sau
    }
}
```

Có thể xoá luôn hàm `DaBiGoKhoiTrang` vì không còn dùng.

Ghi chú:
- Bên trong `wait.Until` mình dùng `driver.FindElements` thay cho `Tim(...)`. `Tim` đã có `wait` riêng, lồng hai `wait` vào nhau thì có thể bị treo tới 10 giây ở mỗi vòng. `FindElements` không ném lỗi khi không tìm thấy mà trả về danh sách rỗng, rất hợp để làm điều kiện chờ. Nếu trong `BasePage` biến driver tên khác thì bạn đổi lại cho khớp.
- Nếu muốn chắc hơn nữa, có thể chờ thêm số trên huy hiệu giảm đi. Lưu ý khi giỏ trống thì `.shopping_cart_badge` **biến mất hẳn**. Vì vậy `LaySoTrenHuyHieu()` phải dùng `FindElements(...)` và trả về `0` khi danh sách rỗng, không dùng `Tim(...)`. Nếu không, assertion `Is.EqualTo(0)` trong test cũng sẽ timeout.

## (3) Vì sao cách sửa này chạy ổn định

1. **Không phụ thuộc cách framework cập nhật DOM.** Dù React dùng lại node, thay node mới hay gỡ hẳn, kết quả cuối cùng mà người dùng thấy vẫn là "không còn dòng `.cart_item` nào mang tên này". Mình chờ đúng kết quả đó, nên điều kiện chắc chắn sẽ thành `true` khi việc xoá hoàn tất. `div.removed_cart_item` không khớp selector `.cart_item`, nên nó không bị tính.
2. **Mỗi lần poll đều tìm lại phần tử.** Không giữ `IWebElement` qua nhiều vòng, nên không còn chuyện dựa vào một tham chiếu có thể còn sống hoặc đã chết.
3. **Bắt các ngoại lệ tạm thời.** Nếu poll rơi đúng lúc DOM đang đổi, `StaleElementReferenceException` hoặc `NoSuchElementException` được coi là "chưa xong" và `WebDriverWait` thử lại ở vòng sau (mặc định 500 ms). Mình bắt ngay trong hàm thay vì gọi `wait.IgnoreExceptionTypes(...)`, để không làm thay đổi đối tượng `wait` dùng chung của `BasePage`.
4. **Đúng quy định của dự án.** Chỉ dùng explicit wait, không có `Thread.Sleep`. Test chạy nhanh nhất có thể: thường điều kiện đúng ngay ở lần poll đầu hoặc thứ hai, và chỉ thật sự chờ khi trang chậm.
5. **Khi lỗi thì thông báo có nghĩa hơn.** Nếu sản phẩm thật sự không bị xoá (nút hỏng, locator sai), test vẫn timeout. Lúc đó nó phản ánh đúng một lỗi chức năng, chứ không phải lỗi do giả định sai về DOM như trước.
