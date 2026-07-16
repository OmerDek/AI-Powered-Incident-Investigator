using AIIncidentInvestigator.Models;

namespace AIIncidentInvestigator.Services;

/// <summary>
/// Stand-in for Omer's AI Analysis Service until POST /api/analysis/analyze is live.
/// Returns data in the exact shape the real service will respond with.
/// </summary>
public class MockAiAnalysisClient : IAiAnalysisClient
{
    public async Task<InvestigationSummary> AnalyzeAsync(AgentInfo agent, List<LogFileContent> logFiles)
    {
        await Task.Delay(1500);

        return new InvestigationSummary
        {
            IncidentOverview = "Data collection stopped at 14:03.",
            InvestigationFindings = new()
            {
                "Authentication token refresh failed.",
                "Multiple retry attempts were unsuccessful."
            },
            LikelyRootCause = "Authentication token refresh failure.",
            Confidence = 89,
            Severity = "High",
            Timeline =
            [
                new TimelineEvent { Timestamp = "14:01", Event = "Authentication token refresh failed." },
                new TimelineEvent { Timestamp = "14:02", Event = "Retry attempt 1 of 3 failed." },
                new TimelineEvent { Timestamp = "14:03", Event = "Data collection stopped." }
            ],
            Evidence =
            [
                "Authentication failed",
                "Retry failed",
                "Collection stopped"
            ],
            RecommendedActions =
            [
                "Verify token refresh flow",
                "Check IdP connectivity"
            ],
            RelevantComponents =
            [
                "AuthenticationManager",
                "SessionManager"
            ]
        };
    }
}
