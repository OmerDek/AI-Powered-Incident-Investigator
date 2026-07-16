namespace AIIncidentInvestigator.Services;

public interface ILogFileService
{
    Task<List<string>> GetAvailableLogFilesAsync(string agentName);
    Task<string> GetLogFileContentAsync(string agentName, string fileName);
    Task<bool> HasLogsAsync(string agentName);
}
