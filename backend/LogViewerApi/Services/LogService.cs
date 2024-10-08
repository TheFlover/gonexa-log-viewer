using System.Text.Json;
using LogViewerApi.Models;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;

namespace LogViewerApi.Services;

public class LogService : ILogService
{
    private readonly string _jsonFilePath;
    private readonly string _generatedFilePath;
    private readonly string _modelFilePath;

    // Initialize file paths from settings and environment variables
    public LogService(IOptions<LogSettings> settings, IWebHostEnvironment environment)
    {
        _jsonFilePath = Path.Combine(environment.ContentRootPath, settings.Value.JsonFilePath);
        _generatedFilePath = Path.Combine(environment.ContentRootPath, settings.Value.GeneratedFilePath);
        _modelFilePath = Path.Combine(environment.ContentRootPath, settings.Value.ModelFilePath);
    }

    // Fetch all logs asynchronously by reading and deserializing the JSON file
    public async Task<IEnumerable<LogEntry>> GetAllLogsAsync()
    {
        var jsonContent = await File.ReadAllTextAsync(_jsonFilePath);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        return JsonSerializer.Deserialize<IEnumerable<LogEntry>>(jsonContent, options) 
            ?? Enumerable.Empty<LogEntry>();
    }

    // Download a generated file for a specific log ID
    public async Task<(Stream FileStream, string ContentType, string FileName)> DownloadGeneratedFileAsync(string logId)
    {
        // Get log by ID, throw if not found
        var logs = await GetAllLogsAsync();
        var log = logs.FirstOrDefault(log => log.Id == logId)
            ?? throw new KeyNotFoundException($"Log ID '{logId}' not found.");

        // Throw if the generated file does not exist
        if (!File.Exists(_generatedFilePath))
        {
            throw new FileNotFoundException($"Generated file not found for log ID '{logId}'");
        }

        // Determine the content type and open the file for reading
        var contentType = GetContentType(_generatedFilePath);
        var fileStream = new FileStream(_generatedFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        
        var newFileName = $"GeneratedDocument_{logId}.pdf";
        
        return (fileStream, contentType, newFileName);
    }

    // Download a model file for a specific log ID
    public async Task<(Stream FileStream, string ContentType, string FileName)> DownloadModelFileAsync(string logId)
    {
        // Get log by ID, throw if not found
        var logs = await GetAllLogsAsync();
        var log = logs.FirstOrDefault(log => log.Id == logId)
            ?? throw new KeyNotFoundException($"Log ID '{logId}' not found.");

        // Throw if the model file does not exist
        if (!File.Exists(_modelFilePath))
        {
            throw new FileNotFoundException($"Model file not found for log ID '{logId}'");
        }

        // Determine the content type and open the file for reading
        var contentType = GetContentType(_modelFilePath);
        var fileStream = new FileStream(_modelFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        
        var newFileName = $"ModelDocument_{logId}.pdf";
        
        return (fileStream, contentType, newFileName);
    }

    // Get the content type based on the file extension, defaulting to 'application/octet-stream'
    private string GetContentType(string filePath)
    {
        var provider = new FileExtensionContentTypeProvider();
        return provider.TryGetContentType(filePath, out var contentType) 
            ? contentType 
            : "application/octet-stream";
    }

    // Retry generation for a specific log ID
    public async Task<LogEntry> RetryGenerationAsync(string logId)
    {
        // Get log by ID, throw if not found
        var logs = await GetAllLogsAsync();
        var log = logs.FirstOrDefault(log => log.Id == logId) 
            ?? throw new KeyNotFoundException($"Log with ID {logId} not found.");

        // Can only retry failed generations
        if (log.GenerationStatus != "FAILED")
        {
            throw new InvalidOperationException("Can only retry failed generations.");
        }

        // Simulate generation by updating the log status
        log = log with
        {
            GenerationStatus = "SUCCESS",
            GenerationEndsAt = DateTime.UtcNow,
            GenerationStartsAt = DateTime.UtcNow.AddSeconds(-log.GenerationDuration)
        };

        return log;
    }
}