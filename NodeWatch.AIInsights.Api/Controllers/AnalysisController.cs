using Microsoft.AspNetCore.Mvc;
using NodeWatch.AIInsights.Api.Models;
using NodeWatch.AIInsights.Api.Services;

namespace NodeWatch.AIInsights.Api.Controllers;

[ApiController]
[Route("api/analysis")]
public class AnalysisController : ControllerBase
{
    private readonly IAiAnalysisService _analysisService;

    public AnalysisController(IAiAnalysisService analysisService)
    {
        _analysisService = analysisService;
    }

    [HttpPost("analyze")]
    [ProducesResponseType(typeof(InvestigationSummary), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Analyze([FromBody] AnalyzeRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.AgentName))
            return BadRequest("AgentName is required.");

        if (request.LogFiles == null || request.LogFiles.Count == 0)
            return BadRequest("At least one log file is required.");

        if (request.LogFiles.Any(f => string.IsNullOrWhiteSpace(f.FileName) || string.IsNullOrWhiteSpace(f.Content)))
            return BadRequest("Each log file must have a non-empty FileName and Content.");

        var result = await _analysisService.AnalyzeAsync(request);
        return Ok(result);
    }
}
