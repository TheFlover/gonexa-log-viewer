using LogViewerApi.Models;

namespace LogViewerApi.Services;

public interface ILogService
{
    Task<IEnumerable<LogEntry>> GetAllLogsAsync();
    Task<(Stream FileStream, string ContentType, string FileName)> DownloadGeneratedFileAsync(string logId);
    Task<(Stream FileStream, string ContentType, string FileName)> DownloadModelFileAsync(string logId);
}