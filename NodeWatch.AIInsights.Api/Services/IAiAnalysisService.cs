using NodeWatch.AIInsights.Api.Models;

namespace NodeWatch.AIInsights.Api.Services;

public interface IAiAnalysisService
{
    Task<InvestigationSummary> AnalyzeAsync(AnalyzeRequest request);
}
