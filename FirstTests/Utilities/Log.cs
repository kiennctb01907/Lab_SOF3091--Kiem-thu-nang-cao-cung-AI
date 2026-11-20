namespace FirstTests.Utilities;

/// <summary>
/// Ghi nhật ký có dấu thời gian + mức độ. Mỗi dòng in ra TestContext.Progress (thấy ngay trên console khi chạy
/// dotnet test -l "console;verbosity=detailed") và ghi thêm vào file Logs/nhatky_yyyyMMdd.log trong thư mục build
/// (mỗi lần chạy mở đầu bằng một dòng phân cách).
/// File test không gọi lớp này, nhật ký đến từ BaseTest.
/// </summary>
public static class Log
{
    // Một file cho cả ngày. Khi chạy song song nhiều luồng cùng ghi nên phải khóa lại.
    private static readonly object khoaGhi = new();
    private static readonly string duongDanFile = TaoDuongDanFile();
    private static bool daGhiDauLanChay;

    public static string DuongDanFile => duongDanFile;

    public static void Info(string noiDung) => Ghi("INFO ", noiDung);

    public static void Warn(string noiDung) => Ghi("WARN ", noiDung);

    public static void Error(string noiDung) => Ghi("ERROR", noiDung);

    private static void Ghi(string mucDo, string noiDung)
    {
        // Mã luồng giúp nhìn ra ca nào chạy trên luồng nào khi bật song song
        string dong = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [{mucDo}] [luồng {Environment.CurrentManagedThreadId,2}] {noiDung}";
        TestContext.Progress.WriteLine(dong);
        lock (khoaGhi)
        {
            if (!daGhiDauLanChay)
            {
                GhiFile($"===== Lần chạy lúc {DateTime.Now:HH:mm:ss} =====");
                daGhiDauLanChay = true;
            }
            GhiFile(dong);
        }
    }

    /// <summary>
    /// Ghi 1 dòng vào file. Chương trình khác (vd Visual Studio đang mở file log) có thể giữ file trong chốc lát,
    /// nên thử lại vài lần; vẫn không được thì bỏ qua dòng đó trong file. Ghi log hỏng không được làm test rớt.
    /// </summary>
    private static void GhiFile(string dong)
    {
        for (int lan = 1; lan <= 5; lan++)
        {
            try
            {
                File.AppendAllText(duongDanFile, dong + Environment.NewLine);
                return;
            }
            catch (IOException) when (lan < 5)
            {
                Thread.Sleep(50);
            }
            catch (IOException ex)
            {
                TestContext.Progress.WriteLine($"[Log] Không ghi được file nhật ký: {ex.Message}");
            }
        }
    }

    private static string TaoDuongDanFile()
    {
        string thuMuc = Path.Combine(AppContext.BaseDirectory, "Logs");
        Directory.CreateDirectory(thuMuc);
        return Path.Combine(thuMuc, $"nhatky_{DateTime.Now:yyyyMMdd}.log");
    }
}
