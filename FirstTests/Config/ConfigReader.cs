using Microsoft.Extensions.Configuration;

namespace FirstTests.Config;

/// <summary>
/// Đọc Config/appsettings.json một lần rồi dùng lại cho mọi test.
/// Đường dẫn lấy theo AppContext.BaseDirectory (luôn là bin/Debug/...) chứ không theo thư mục làm việc hiện tại,
/// vì thư mục làm việc thay đổi tùy chạy bằng Test Explorer, dotnet test hay CI.
/// </summary>
public static class ConfigReader
{
    private static readonly Lazy<TestConfig> cauHinh = new(DocFile);

    public static TestConfig Get() => cauHinh.Value;

    private static TestConfig DocFile()
    {
        string duongDan = Path.Combine(AppContext.BaseDirectory, "Config", "appsettings.json");
        if (!File.Exists(duongDan))
        {
            throw new FileNotFoundException(
                "Không thấy appsettings.json trong thư mục build. Kiểm tra CopyToOutputDirectory trong FirstTests.csproj.", duongDan);
        }

        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile(Path.Combine("Config", "appsettings.json"), optional: false)
            .Build();

        return config.Get<TestConfig>()
            ?? throw new InvalidOperationException("appsettings.json rỗng, không đọc được cấu hình.");
    }
}
