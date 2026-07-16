using NodeWatch.AIInsights.Api.Models;

namespace NodeWatch.AIInsights.Api.Services;

public class PromptBuilderService : IPromptBuilderService
{
    public string BuildPrompt(AnalyzeRequest request)
    {
        var logFilesBlock = string.Join("\n\n", request.LogFiles.Select(f =>
            $"--- File: {f.FileName} ---\n{f.Content}"));

        var problemDescriptionBlock = string.IsNullOrWhiteSpace(request.ProblemDescription)
            ? "No specific problem description was provided — investigate the logs for any notable issues."
            : request.ProblemDescription;

        return $$"""
            You are an AI operations analyst. Analyze the following log files from a monitored agent and return a structured investigation summary.

            Agent Metadata:
              Agent Name:     {{request.AgentName}}
              Team:           {{request.Team}}
              Client Version: {{request.ClientVersion}}
              Config Version: {{request.ConfigVersion}}

            Reported Problem:
              {{problemDescriptionBlock}}

            Log Files ({{request.LogFiles.Count}} file(s)):
            {{logFilesBlock}}

            Instructions:
            1. Focus the investigation on the reported problem above — use it to prioritize which log lines and patterns matter most, while still correlating across ALL provided log files.
            2. Identify the root cause hypothesis, tracing the failure chain across files where relevant.
            3. Generate a concise incident summary describing what happened and its impact.
            4. List key investigation findings — each finding should be a specific, evidence-backed observation.
            5. Extract evidence: exact log lines or patterns that support your conclusions. Include the source file name.
            6. Build a timeline of key events in chronological order, sourcing each event from its log file.
            7. List all relevant components (services, modules, processes) involved in the incident.
            8. List the log file names that contain the most relevant evidence.
            9. Provide actionable recommendations to resolve and prevent recurrence.
            10. Assign a confidence score (0-100) based on the quality and completeness of the evidence.
            11. Assign a severity level: Low, Medium, High, or Critical.

            Respond ONLY with valid JSON matching this exact schema — no explanation, no markdown, no code fences:
            {
              "incidentSummary": "string",
              "investigationFindings": ["string"],
              "rootCauseHypothesis": "string",
              "confidence": 0,
              "severity": "string",
              "timeline": [
                { "timestamp": "string", "event": "string" }
              ],
              "recommendations": ["string"],
              "relevantComponents": ["string"],
              "relevantLogFiles": ["string"],
              "evidence": ["string"]
            }
            """;
    }
}
