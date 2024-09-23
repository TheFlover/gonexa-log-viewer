using LogViewerApi.Models;

namespace LogViewerApi.Services;

public interface ILogService
{
    Task<IEnumerable<LogEntry>> GetAllLogsAsync();
}