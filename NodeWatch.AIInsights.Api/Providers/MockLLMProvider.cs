using NodeWatch.AIInsights.Api.Models;

namespace NodeWatch.AIInsights.Api.Providers;

public class MockLLMProvider : ILLMProvider
{
    public Task<InvestigationSummary> AnalyzeAsync(AnalyzeRequest request, string prompt)
    {
        var fileNames = request.LogFiles.Select(f => f.FileName).ToList();

        var summary = new InvestigationSummary
        {
            IncidentSummary = $"Agent '{request.AgentName}' (team: {request.Team}, client v{request.ClientVersion}, config v{request.ConfigVersion}) experienced authentication failures leading to collection stoppage.",
            InvestigationFindings = new List<string>
            {
                "Authentication failures were the first recorded symptom across multiple log files.",
                "Retry logic exhausted its attempt limit without a successful connection.",
                "Collection was halted as a direct result of unresolved authentication errors.",
                "No configuration change was detected in the config version that would explain the failure."
            },
            RootCauseHypothesis = "Authentication service unavailability caused cascading retry exhaustion and eventual collection shutdown.",
            Confidence = 82,
            Severity = "High",
            Timeline = new List<TimelineEvent>
            {
                new() { Timestamp = "T+0s",  Event = "Authentication failed (source: " + (fileNames.ElementAtOrDefault(0) ?? "unknown") + ")" },
                new() { Timestamp = "T+5s",  Event = "Retry attempt initiated" },
                new() { Timestamp = "T+15s", Event = "Retry failed" },
                new() { Timestamp = "T+16s", Event = "Collection stopped" }
            },
            Recommendations = new List<string>
            {
                "Verify authentication service health and connectivity.",
                "Review credential rotation schedules for this agent.",
                "Implement a circuit breaker to prevent cascading failures.",
                "Add alerting for repeated authentication failures across all agents."
            },
            RelevantComponents = new List<string>
            {
                "AuthenticationService",
                "CollectionAgent",
                "RetryPolicy"
            },
            RelevantLogFiles = fileNames,
            Evidence = new List<string>
            {
                "ERROR Authentication failed",
                "WARN Retry failed",
                "ERROR Collection stopped"
            }
        };

        return Task.FromResult(summary);
    }
}
