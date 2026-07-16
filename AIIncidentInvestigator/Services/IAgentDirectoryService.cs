using AIIncidentInvestigator.Models;

namespace AIIncidentInvestigator.Services;

public interface IAgentDirectoryService
{
    Task<List<AgentInfo>> GetAgentsAsync();
    Task<AgentInfo?> GetAgentAsync(string agentName);
}
