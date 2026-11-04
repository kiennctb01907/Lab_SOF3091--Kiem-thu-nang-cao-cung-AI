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
| `08_git_log_graph.png` | `git log --graph`: các branch thành viên và các lần merge Pull Request | C |
| `09_pull_requests_merged.jpg` | 5 Pull Request của nhóm đã được merge | C |

Các file `log_*.txt` là output dạng chữ của các lệnh trên.

> Ghi chú: trong `06_dotnet_test_passed.png`, dòng tiếng Việt in ra bị lỗi font vì console Windows không hiển thị đúng UTF-8 của tiến trình test. Kết quả Passed vẫn hiển thị rõ, và nội dung đầy đủ có trong `log_dotnet_test.txt`.
