using AIIncidentInvestigator.Models;

namespace AIIncidentInvestigator.Services;

public interface IAiAnalysisClient
{
    Task<InvestigationSummary> AnalyzeAsync(AgentInfo agent, List<LogFileContent> logFiles, string? problemDescription = null);
}
