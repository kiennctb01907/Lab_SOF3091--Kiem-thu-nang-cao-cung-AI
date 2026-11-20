// Lab 6 - Phần D: bật chạy song song theo từng fixture (mỗi class test chạy trên một luồng riêng).
// Các ca trong cùng một class vẫn chạy lần lượt, vì NUnit chỉ tạo MỘT đối tượng cho mỗi fixture
// nên nếu cho các ca cùng class chạy song song (ParallelScope.Children) thì chúng sẽ dùng chung field driver.
// LevelOfParallelism(4): máy có 12 luồng CPU nhưng mỗi luồng mở một Chrome, để 4 cho máy khỏi nghẹt.
[assembly: Parallelizable(ParallelScope.Fixtures)]
[assembly: LevelOfParallelism(4)]
