namespace AIIncidentInvestigator.Models;

public class InvestigationSummary
{
    public string IncidentOverview { get; set; } = "";
    public List<string> InvestigationFindings { get; set; } = new();
    public string LikelyRootCause { get; set; } = "";
    public int Confidence { get; set; }
    public string Severity { get; set; } = "";
    public List<TimelineEvent> Timeline { get; set; } = new();
    public List<string> Evidence { get; set; } = new();
    public List<string> RecommendedActions { get; set; } = new();
    public List<string> RelevantComponents { get; set; } = new();
}
