using System.Text.Json;
using LogViewerApi.Models;
using Microsoft.Extensions.Options;

namespace LogViewerApi.Services;

public class LogService : ILogService
{
    private readonly string _jsonFilePath;

    public LogService(IOptions<LogSettings> settings, IWebHostEnvironment environment)
    {
        _jsonFilePath = Path.Combine(environment.ContentRootPath, settings.Value.FilePath);
    }

    public async Task<IEnumerable<LogEntry>> GetAllLogsAsync()
    {
        var jsonContent = await File.ReadAllTextAsync(_jsonFilePath);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        return JsonSerializer.Deserialize<IEnumerable<LogEntry>>(jsonContent, options) ?? Enumerable.Empty<LogEntry>();
    }
}