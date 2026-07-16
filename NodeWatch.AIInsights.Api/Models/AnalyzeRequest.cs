namespace NodeWatch.AIInsights.Api.Models;

public class AnalyzeRequest
{
    public string AgentName { get; set; } = string.Empty;
    public string Team { get; set; } = string.Empty;
    public string ClientVersion { get; set; } = string.Empty;
    public string ConfigVersion { get; set; } = string.Empty;
    public List<LogFile> LogFiles { get; set; } = new();
}
