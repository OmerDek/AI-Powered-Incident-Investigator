using NodeWatch.AIInsights.Api.Models;

namespace NodeWatch.AIInsights.Api.Services;

public interface IPromptBuilderService
{
    string BuildPrompt(AnalyzeRequest request);
}
