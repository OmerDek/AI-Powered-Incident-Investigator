using AIIncidentInvestigator.Models;

namespace AIIncidentInvestigator.Services;

/// <summary>
/// Stand-in for the real Node Watch fleet directory until it's wired to the actual
/// Node Watch / CXone Discover data source. Swap for a real implementation later -
/// nothing on the UI side needs to change, same as the other Mock/Real service pairs.
/// </summary>
public class MockAgentDirectoryService : IAgentDirectoryService
{
    private static readonly List<AgentInfo> Agents = new()
    {
         new()
        {
            AgentName = "Akshay Jagtap", Team = "Team - 34", ClientVersion = "2.1.35",
            ConfigVersion = "CFG.25.1.1.92", Module = "DataCollection, RTA",
            ClientStatus = "Offline", StatusDetail = "Offline (1d 1h)"        },
        new()
        {
            AgentName = "Jayesh Sapariya", Team = "Team - 89", ClientVersion = "2.1.67",
            ConfigVersion = "CFG.25.1.1.56", Module = "Data Collection",
            ClientStatus = "Offline", StatusDetail = "Offline (3d 1h)"        },
        new()
        {
            AgentName = "Lior Epstien", Team = "Team - 95", ClientVersion = "2.1.83",
            ConfigVersion = "CFG.25.1.1.76", Module = "Data Collection",
            ClientStatus = "Offline", StatusDetail = "Offline (5d 1h)"        },
        new()
        {
            AgentName = "Ghanshyam Jadhav", Team = "Team - 56", ClientVersion = "2.1.345",
            ConfigVersion = "CFG.25.1.1.72", Module = "DataCollection",
            ClientStatus = "Offline", StatusDetail = "Offline (2d 1h)"        },
        new()
        {
            AgentName = "Omer Dekel", Team = "CI Client Test", ClientVersion = "1.3.289",
            ConfigVersion = "CFG.26.3.338", Module = "DataCollection",
            ClientStatus = "Offline", StatusDetail = "Offline (3d 2h)"        },           
        new()
        {
            AgentName = "Alex Carter", Team = "Team - 1", ClientVersion = "2.1.52",
            ConfigVersion = "CFG.25.1.1.52", Module = "Data Collection",
            ClientStatus = "Offline", StatusDetail = "Offline (22d 1h)"        },
        new()
        {
            AgentName = "Priya Nair", Team = "Test team P2", ClientVersion = "2.1.51",
            ConfigVersion = "CFG.25.1.1.91", Module = "DataCollection",
            ClientStatus = "Offline", StatusDetail = "Offline (16d 2h)"        },
        new()
        {
            AgentName = "Sam Wu", Team = "DefaultTeam", ClientVersion = "2.1.51",
            ConfigVersion = "CFG.25.1.1.95", Module = "DataCollection, RTA",
            ClientStatus = "Offline", StatusDetail = "Offline (3 months)"        },
        new()
        {
            AgentName = "Jordan Blake", Team = "Team - 155", ClientVersion = "2.1.51",
            ConfigVersion = "CFG.25.1.1.96", Module = "DataCollection",
            ClientStatus = "Active", StatusDetail = "Active"        },
        new()
        {
            AgentName = "Riley Chen", Team = "Team - 1", ClientVersion = "2.1.50",
            ConfigVersion = "CFG.25.1.1.86", Module = "DataCollection",
            ClientStatus = "Inactive", StatusDetail = "Inactive (2h)"        },
    };

    public Task<List<AgentInfo>> GetAgentsAsync() => Task.FromResult(Agents.ToList());

    public Task<AgentInfo?> GetAgentAsync(string agentName) =>
        Task.FromResult(Agents.FirstOrDefault(a => a.AgentName == agentName));
}
