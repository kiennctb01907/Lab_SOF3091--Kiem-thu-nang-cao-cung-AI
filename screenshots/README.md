# Ảnh minh chứng

| File | Nội dung | Phần |
|---|---|---|
| `01_dotnet_version.png` | Terminal chạy `dotnet --version` → `10.0.301` | A |
| `02_dotnet_info.png` | Terminal chạy `dotnet --info` | A |
| `03_visual_studio_csproj.png` | Visual Studio 2026 mở `FirstTests.csproj`, có gói `Selenium.WebDriver` 4.52.0 | A |
| `04_visual_studio_test_run.png` | Visual Studio: code test, thanh trạng thái *Test run finished: 2 Tests (2 Passed, 0 Failed)* | B |
| `05_test_explorer_passed.png` | Test Explorer: 2 test **Passed** | B |
| `06_dotnet_test_passed.png` | Terminal chạy `dotnet test` → *Passed: 2* | B |
| `07_commit_history_github.jpg` | Lịch sử commit trên GitHub (branch `main`) | C |

## Lab 5

| File | Nội dung | Phần |
|---|---|---|
| `lab5_A1_appsettings_va_cau_truc.png` | Visual Studio: `Config/appsettings.json` và cấu trúc thư mục mới trong Solution Explorer | A |
| `lab5_A2_csproj_copy_to_output.png` | `FirstTests.csproj`: 2 gói Configuration và `CopyToOutputDirectory` cho 2 file JSON | A |
| `lab5_A3_configreader.png` | `ConfigReader.cs` đọc file theo `AppContext.BaseDirectory` | A |
| `lab5_A4_find_khong_con_url.png` | Find in Files `https?://` trong `*.cs` → 0 kết quả | A |
| `lab5_B1_driverfactory_create.png` | `DriverFactory.Create` trả về `IWebDriver`, báo lỗi rõ khi sai tên | B |
| `lab5_B2_driverfactory_options.png` | Options riêng cho Chrome / Edge / Firefox, headless áp cho cả 3 | B |
| `lab5_B3_chrome_dang_chay.png` | Chrome đang chạy ca data-driven trên saucedemo | B |
| `lab5_B4_chrome_ket_qua.png` | `"Browser": "Chrome"` → Run All 19/19 Passed | B |
| `lab5_B5_edge_dang_chay.png` | Edge đang chạy ca data-driven trên saucedemo | B |
| `lab5_B6_edge_ket_qua.png` | `"Browser": "Edge"` → Run All 19/19 Passed | B |
| `lab5_B7_sai_ten_trinh_duyet.png` | `"Browser": "Safari"` → NotSupportedException nêu đúng tên | B |
| `lab5_C1_basetest_setup.png` | `Core/BaseTest.cs`: SetUp đọc config, tạo driver, mở BaseUrl | C |
| `lab5_C2_basetest_try_finally.png` | TearDown: chụp ảnh trong try, Quit/Dispose trong finally | C |
| `lab5_C3_find_khong_con_setup.png` | Find in Files `[SetUp]`/`[TearDown]`/`new ChromeDriver` trong `Tests` → 0 kết quả | C |
| `lab5_C4_test_co_y_rot.png` | Test cố ý rớt: Standard Output "Đã chụp ảnh khi test rớt" + file đính kèm | C |
| `lab5_C5_anh_tu_dong_chup_khi_rot.png` | Ảnh do `ScreenshotHelper` tự chụp lúc test rớt | C |
| `lab5_D1_testcasesource_setname.png` | `[TestCaseSource(nameof(...))]` + `SetName` trong `LoginTests.cs` | D |
| `lab5_D2_basepage_cohienthi.png` | `BasePage.CoHienThi()` dùng FindElements, chờ 3 giây, trả false | D |
| `lab5_D3_inventory_dahienthi.png` | `InventoryPage.DaHienThi()` đã sửa | D |
| `lab5_D4_users_json.png` | `Data/users.json` | D |
| `lab5_D5_truoc_8_ca.png` | Trước khi thêm dữ liệu: 8 ca DangNhap Passed | D |
| `lab5_D6_sau_9_ca.png` | Sau khi thêm 1 bộ vào JSON: 9 ca DangNhap Passed | D |
| `lab5_D7_commit_chi_sua_users_json.png` | Commit thêm dữ liệu chỉ sửa `users.json` | D |
| `lab5_E1_chay_thu_du_lieu_ai.png` | Chạy thử 10 bộ AI sinh: 8 Passed, 2 Failed | E |
| `lab5_E2_chi_tiet_ca_ai_sai.png` | Chi tiết ca AI 05 `STANDARD_USER` rớt | E |
| `lab5_F_run_all_chrome.png` | Run All cuối cùng với Chrome: 20/20 Passed | — |

## Lab 6

| File | Nội dung | Phần |
|---|---|---|
| `lab6_A1_category_trong_code.png` | `[Category]` gắn trong LoginTests.cs | A |
| `lab6_A2_test_explorer_trait_smoke.png` | Test Explorer lọc `Trait:"Smoke"`: 5 ca | A |
| `lab6_A3_test_explorer_tru_slow.png` | Test Explorer lọc `-Trait:"Slow"`: 23 ca | A |
| `lab6_A4_dotnet_test_category_smoke.png` | Terminal VS: `dotnet test --filter "Category=Smoke"` → 5 succeeded | A |
| `lab6_A5_dotnet_test_category_khac_slow.png` | Terminal VS: `dotnet test --filter "Category!=Slow"` → 23 succeeded | A |
| `lab6_B1_lop_log.png` | Utilities/Log.cs | B |
| `lab6_B2_basetest_ghi_log.png` | BaseTest ghi BẮT ĐẦU / OK / RỚT | B |
| `lab6_B3_find_khong_log_trong_tests.png` | Find in Files `Log.` trong Tests: 0 kết quả | B |
| `lab6_B4_nhat_ky_co_ca_rot.png` | Nhật ký thật có [INFO] và [ERROR] | B |
| `lab6_C1_dotnet_test_trx.png` | `dotnet test --logger "trx;LogFileName=ketqua.trx"` | C |
| `lab6_C2_trxlog2html.png` | trxlog2html sinh ketqua.html | C |
| `lab6_C3_bao_cao_html.png` | Trang báo cáo HTML: 27 ca, 25 passed, 0 failed | C |
| `lab6_D1_tuan_tu_1p7.png` | Chạy tuần tự: 25 Passed, 1,7 phút | D |
| `lab6_D2_parallel_settings.png` | ParallelSettings.cs | D |
| `lab6_D3_song_song_rot_ngau_nhien.png` | Song song lần 3 trước khi sửa: 1 ca rớt | D |
| `lab6_D4_chi_tiet_ca_rot_ioexception.png` | Chi tiết: IOException khi ghi file log | D |
| `lab6_D5_song_song_sau_sua_lan1.png` | Song song sau khi sửa lần 1: 25 Passed, 1,3 phút | D |
| `lab6_D6_song_song_sau_sua_lan2.png` | Song song sau khi sửa lần 2: 25 Passed, 1,4 phút | D |
| `lab6_D7_song_song_sau_sua_lan3.png` | Song song sau khi sửa lần 3: 25 Passed, 1,3 phút | D |
| `lab6_E1_chay_thu_11_ca_ai.png` | Chạy thử 11 ca AI: 5 Passed, 6 Failed | E |
| `lab6_E2_checkout_tests_da_loc.png` | CheckoutTests.cs: 5 ca đã lọc | E |
