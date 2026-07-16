namespace AIIncidentInvestigator.Models;

public class AgentInfo
{
    public string AgentName { get; set; } = "";
    public string Team { get; set; } = "";
    public string ClientVersion { get; set; } = "";
    public string ConfigVersion { get; set; } = "";
    public string Module { get; set; } = "";
    public string ClientStatus { get; set; } = "";   // Active / Inactive / Offline
    public string StatusDetail { get; set; } = "";   // e.g. "Offline (3d 2h)"
    public string LogStatus { get; set; } = "";      // e.g. "Successful"
}
