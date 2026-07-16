using System.Net.Http.Json;
using AIIncidentInvestigator.Models;

namespace AIIncidentInvestigator.Services;

/// <summary>
/// Calls Omer's AI Analysis Service. Swap MockAiAnalysisClient for this in Program.cs
/// once POST /api/analysis/analyze is live, and set AiService:BaseUrl in appsettings.json.
/// </summary>
public class RealAiAnalysisClient : IAiAnalysisClient
{
    private readonly HttpClient _http;
    private readonly IConfiguration _configuration;

    public RealAiAnalysisClient(HttpClient http, IConfiguration configuration)
    {
        _http = http;
        _configuration = configuration;
    }

    public async Task<InvestigationSummary> AnalyzeAsync(AgentInfo agent, List<LogFileContent> logFiles)
    {
        var baseUrl = _configuration["AiService:BaseUrl"]
            ?? throw new InvalidOperationException("AiService:BaseUrl is not configured in appsettings.json");

        var response = await _http.PostAsJsonAsync($"{baseUrl}/api/analysis/analyze", new
        {
            agentName = agent.AgentName,
            team = agent.Team,
            clientVersion = agent.ClientVersion,
            configVersion = agent.ConfigVersion,
            logFiles = logFiles.Select(f => new { fileName = f.FileName, content = f.Content })
        });

        response.EnsureSuccessStatusCode();

        var remote = await response.Content.ReadFromJsonAsync<RemoteInvestigationSummary>()
            ?? throw new InvalidOperationException("AI Analysis Service returned an empty response.");

        return new InvestigationSummary
        {
            IncidentOverview = remote.IncidentSummary,
            InvestigationFindings = remote.InvestigationFindings,
            LikelyRootCause = remote.RootCauseHypothesis,
            Confidence = remote.Confidence,
            Severity = remote.Severity,
            Timeline = remote.Timeline,
            Evidence = remote.Evidence,
            RecommendedActions = remote.Recommendations,
            RelevantComponents = remote.RelevantComponents
        };
    }

    // Matches Omer's AI Analysis Service response field names exactly (NodeWatch.AIInsights.Api,
    // Models/InvestigationSummary.cs) - some names differ from our own InvestigationSummary model,
    // so this DTO exists purely to translate between the two without requiring changes on his side.
    private class RemoteInvestigationSummary
    {
        public string IncidentSummary { get; set; } = "";
        public List<string> InvestigationFindings { get; set; } = new();
        public string RootCauseHypothesis { get; set; } = "";
        public int Confidence { get; set; }
        public string Severity { get; set; } = "";
        public List<TimelineEvent> Timeline { get; set; } = new();
        public List<string> Recommendations { get; set; } = new();
        public List<string> RelevantComponents { get; set; } = new();
        public List<string> RelevantLogFiles { get; set; } = new();
        public List<string> Evidence { get; set; } = new();
    }
}
