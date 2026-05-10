using Loyalty.Infrastructure.Services;
using Xunit;

namespace Loyalty.UnitTests.InfrastructureTests;

public class FileStorageServiceTests
{
    [Fact]
    public void Save_ShouldCreateLogFile_AndAppendJson()
    {
        var service = new FileStorageService();

        var tempDir = Path.Combine(Path.GetTempPath(), "loyalty-test-logs");
        Directory.CreateDirectory(tempDir);

        var originalCurrent = Directory.GetCurrentDirectory();
        Directory.SetCurrentDirectory(tempDir);

        try
        {
            var data = new
            {
                Value = 123,
                Message = "Test"
            };

            service.Save(data);

            var date = DateTime.UtcNow.ToString("yyyyMMdd");

            var rootPath = Path.GetFullPath(
                Path.Combine(tempDir, "..", "..")
            );

            var filePath = Path.Combine(rootPath, "logs", $"loyalty-log-{date}.json");

            Assert.True(File.Exists(filePath));

            var fileContent = File.ReadAllText(filePath);

            Assert.Contains("123", fileContent);
            Assert.Contains("Test", fileContent);
        }
        finally
        {
            Directory.SetCurrentDirectory(originalCurrent);
            Directory.Delete(tempDir, true);
        }
    }
}