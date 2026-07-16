namespace NodeWatch.AIInsights.Api.Models;

public class InvestigationSummary
{
    public string IncidentSummary { get; set; } = string.Empty;
    public List<string> InvestigationFindings { get; set; } = new();
    public string RootCauseHypothesis { get; set; } = string.Empty;
    public int Confidence { get; set; }
    public string Severity { get; set; } = string.Empty;
    public List<TimelineEvent> Timeline { get; set; } = new();
    public List<string> Recommendations { get; set; } = new();
    public List<string> RelevantComponents { get; set; } = new();
    public List<string> RelevantLogFiles { get; set; } = new();
    public List<string> Evidence { get; set; } = new();
}
