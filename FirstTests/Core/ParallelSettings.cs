// Lab 6 - Phần D: bật chạy song song theo từng fixture (mỗi class test chạy trên một luồng riêng).
// Các ca trong cùng một class vẫn chạy lần lượt, vì NUnit chỉ tạo MỘT đối tượng cho mỗi fixture
// nên nếu cho các ca cùng class chạy song song (ParallelScope.Children) thì chúng sẽ dùng chung field driver.
// LevelOfParallelism(6): bằng số nhân CPU của máy (AMD Ryzen 5 220: 6 nhân, 12 luồng), theo gợi ý của đề.
[assembly: Parallelizable(ParallelScope.Fixtures)]
[assembly: LevelOfParallelism(6)]
