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
| `lab6_A3_test_explorer_tru_slow.png` | Test Explorer lọc `-Trait:"Slow"`: 26 ca | A |
| `lab6_A4_dotnet_test_category_smoke.png` | Terminal VS: `dotnet test --filter "Category=Smoke"` → 5 succeeded | A |
| `lab6_A5_dotnet_test_category_khac_slow.png` | Terminal VS: `dotnet test --filter "Category!=Slow"` → 26 succeeded | A |
| `lab6_B1_lop_log.png` | Utilities/Log.cs | B |
| `lab6_B2_basetest_ghi_log.png` | BaseTest ghi BẮT ĐẦU / OK / RỚT | B |
| `lab6_B3_find_khong_log_trong_tests.png` | Find in Files `Log.` trong Tests: 0 kết quả trên 9 file | B |
| `lab6_B4_nhat_ky_co_ca_rot.png` | File nhật ký mở trong VS: [INFO] OK và [ERROR] RỚT | B |
| `lab6_B5_test_explorer_nhat_ky_mau.png` | Test Explorer lọc `Trait:"NhatKyMau"`: 1 Passed, 1 Failed | B |
| `lab6_B6_dotnet_test_nhat_ky_mau.png` | Terminal VS: nhật ký hiện trực tiếp, có dòng [ERROR] | B |
| `lab6_C1_dotnet_test_trx.png` | `dotnet test --logger "trx;LogFileName=ketqua.trx"` | C |
| `lab6_C2_trxlog2html.png` | Lệnh trxlog2html và file ketqua.html | C |
| `lab6_C3_bao_cao_html.png` | Trang báo cáo HTML: 32 ca, 28 passed, 0 failed | C |
| `lab6_D2_parallel_settings.png` | ParallelSettings.cs (LevelOfParallelism 6) | D |
| `lab6_D3_song_song_rot_ngau_nhien.png` | Song song lần 3 trước khi sửa log: 1 ca rớt | D |
| `lab6_D4_chi_tiet_ca_rot_ioexception.png` | Chi tiết: IOException khi ghi file log | D |
| `lab6_D8_song_song_6_luong_lan1.png` | Run All song song lần 1: 28 Passed | D |
| `lab6_D9_song_song_6_luong_lan2.png` | Run All song song lần 2: 28 Passed | D |
| `lab6_D10_song_song_6_luong_lan3.png` | Run All song song lần 3: 28 Passed | D |
| `lab6_D11_dotnet_test_tuan_tu.png` | Tuần tự (`NUnit.NumberOfTestWorkers=0`): 28 succeeded, 108,6 giây | D |
| `lab6_D12_dotnet_test_song_song.png` | Song song: 28 succeeded, 75,2 giây | D |
| `lab6_E1_chay_thu_ai.png` | Chạy thử 10 ca AI: 10 Passed | E |
| `lab6_E2_checkout_tests_da_loc.png` | CheckoutTests.cs: 8 ca đã lọc, không có [SetUp] | E |

## Lab 7

| File | Nội dung | Phần |
|---|---|---|
| `lab7_A1_prompt_4_thanh_phan.png` | Prompt Phần A mở bằng Markdown Preview của VS: vai trò, ngữ cảnh, yêu cầu, định dạng | A |
| `lab7_A2_ai_tra_loi_16_ca.png` | Bảng 16 test case AI trả về (đoạn đầu) | A |
| `lab7_B1_bang_giu_bo_sua.png` | Bảng đối chiếu GIỮ / BỎ / SỬA với trang thật | B |
| `lab7_C1_prompt_cartpage.png` | Prompt Phần C kèm 5 quy ước và 2 file mẫu | C |
| `lab7_C2_ai_tra_loi_cartpage.png` | Câu trả lời của AI: code CartPage và bảng locator | C |
| `lab7_C3_cartpage_ai_trong_project.png` | CartPage.cs bản gốc của AI trong project, đoạn XoaSanPham | C |
| `lab7_C4_kiem_locator_ai_tren_trang_that.png` | Test Explorer: KiemLocatorCartPageTests Passed, Standard Output 10/10 locator đúng số | C |
| `lab7_C5_dotnet_build.png` | Terminal VS: `dotnet build FirstTests` → Build succeeded | C |
| `lab7_C6_dotnet_test_gio_hang.png` | Terminal VS: `dotnet test` lọc GioHang + ThamDinhAI → 13 succeeded | C |
| `lab7_C7_gio_hang_tests.png` | GioHangTests.cs: phần đầu file | C |
| `lab7_C8_ca_tinh_tong_da_sua.png` | Ca tính tổng đã sửa (gộp TC05, TC07, TC10) | C |
| `lab7_D1_test_explorer_do_truoc_khi_sua.png` | Test Explorer: 10 Passed, 2 Failed, stack trace chỉ CartPage.XoaSanPham line 71 | D |
| `lab7_D2_gia_thuyet_tu_doan.png` | Giả thuyết tự đoán, viết trước khi hỏi AI | D |
| `lab7_D3_prompt_hoi_ai_debug.png` | Prompt gửi AI: stack trace và code | D |
| `lab7_D4_ai_chan_doan.png` | AI chẩn đoán nguyên nhân gốc | D |
| `lab7_D5_xoasanpham_da_sua.png` | XoaSanPham sau khi sửa (CodeLens còn ghi 0/2 passing của lần chạy trước) | D |
| `lab7_D6_ham_cho_moi.png` | Hàm chờ mới ConSanPhamTrongGio | D |
| `lab7_D7_test_explorer_xanh_sau_khi_sua.png` | Test Explorer sau khi sửa: 12/12 Passed | D |
| `lab7_E1_ai_tc15_ky_vong_sai.png` | TC15 trong câu trả lời của AI: kỳ vọng "phải chặn giỏ trống" | E |
| `lab7_E2_ai_cartpage_cho_stale.png` | Ghi chú của AI: "XoaSanPham chờ dòng bị gỡ khỏi DOM (stale)" | E |
