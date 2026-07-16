namespace AIIncidentInvestigator.Services;

/// <summary>
/// Stand-in for real log retrieval until an S3 bucket and credentials are configured.
/// Swap for an ILogFileService implementation backed by IS3LogService once that's available -
/// nothing in the UI needs to change, same as IAiAnalysisClient's Mock/Real split.
/// </summary>
public class MockLogFileService : ILogFileService
{
    private static readonly Dictionary<string, string> Files = new()
    {
        ["DiscoveryAgent.log"] =
            "2026-07-15 10:15 ERROR Authentication failed\n" +
            "2026-07-15 10:16 WARN Retry failed\n" +
            "2026-07-15 10:17 ERROR Collection stopped",
        ["CollectionService.log"] =
            "2026-07-15 10:10 INFO Collection cycle started\n" +
            "2026-07-15 10:14 WARN Queue backlog detected\n" +
            "2026-07-15 10:17 ERROR Collection stopped",
        ["UploadService.log"] =
            "2026-07-15 10:12 INFO Upload batch started\n" +
            "2026-07-15 10:18 ERROR No data to upload",
        ["SystemEvents.log"] =
            "2026-07-15 10:00 INFO Agent started\n" +
            "2026-07-15 10:15 WARN High memory usage\n" +
            "2026-07-15 10:17 ERROR Service degraded",
    };

    public Task<List<string>> GetAvailableLogFilesAsync(string agentName)
        => Task.FromResult(Files.Keys.ToList());

    public Task<string> GetLogFileContentAsync(string agentName, string fileName)
        => Task.FromResult(Files.TryGetValue(fileName, out var content) ? content : "(log file not found)");
}
