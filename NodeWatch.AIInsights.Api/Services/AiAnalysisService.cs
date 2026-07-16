using NodeWatch.AIInsights.Api.Models;
using NodeWatch.AIInsights.Api.Providers;

namespace NodeWatch.AIInsights.Api.Services;

public class AiAnalysisService : IAiAnalysisService
{
    private readonly ILLMProvider _llmProvider;
    private readonly IPromptBuilderService _promptBuilder;

    public AiAnalysisService(ILLMProvider llmProvider, IPromptBuilderService promptBuilder)
    {
        _llmProvider = llmProvider;
        _promptBuilder = promptBuilder;
    }

    public async Task<InvestigationSummary> AnalyzeAsync(AnalyzeRequest request)
    {
        var prompt = _promptBuilder.BuildPrompt(request);
        return await _llmProvider.AnalyzeAsync(request, prompt);
    }
}
