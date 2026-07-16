using NodeWatch.AIInsights.Api.Models;

namespace NodeWatch.AIInsights.Api.Providers;

public interface ILLMProvider
{
    Task<InvestigationSummary> AnalyzeAsync(AnalyzeRequest request, string prompt);
}
