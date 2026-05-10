using System.Text.Json;
using Loyalty.Application.Interfaces;

namespace Loyalty.Infrastructure.Services;

public class FileStorageService : IFileStorageService
{
    public void Save(object data)
    {
        var date = DateTime.UtcNow.ToString("yyyyMMdd");
        var rootPath = Path.GetFullPath(
            Path.Combine(Directory.GetCurrentDirectory(), "..", "..")
        );

        var logDir = Path.Combine(rootPath, "logs");

        Directory.CreateDirectory(logDir);

        var filePath = Path.Combine(
            logDir,
            $"loyalty-log-{date}.json");

        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });

        File.AppendAllText(filePath, json + Environment.NewLine);
    }
}