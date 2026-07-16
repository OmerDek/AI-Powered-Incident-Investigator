using Amazon;
using Amazon.BedrockRuntime;
using Amazon.BedrockRuntime.Model;
using System.Text.Json;
using NodeWatch.AIInsights.Api.Models;

namespace NodeWatch.AIInsights.Api.Providers;

public class BedrockProvider : ILLMProvider
{
    private readonly AmazonBedrockRuntimeClient _client;
    private readonly string _modelId;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public BedrockProvider(IConfiguration config)
    {
        var region = config["AWS:Region"]
            ?? Environment.GetEnvironmentVariable("AWS_REGION")
            ?? "us-west-2";

        _modelId = config["AWS:ModelId"] ?? "openai.gpt-oss-120b-1:0";

        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("AWS_BEARER_TOKEN_BEDROCK")))
            throw new InvalidOperationException(
                "AWS_BEARER_TOKEN_BEDROCK environment variable is not set. " +
                "Set it to the API key from the AWS Bedrock quickstart and restart the app.");

        // The SDK auto-detects AWS_BEARER_TOKEN_BEDROCK and signs requests with it —
        // no explicit credentials needed.
        _client = new AmazonBedrockRuntimeClient(new AmazonBedrockRuntimeConfig
        {
            RegionEndpoint = RegionEndpoint.GetBySystemName(region)
        });
    }

    public async Task<InvestigationSummary> AnalyzeAsync(AnalyzeRequest request, string prompt)
    {
        var converseRequest = new ConverseRequest
        {
            ModelId = _modelId,
            Messages =
            [
                new Message
                {
                    Role = ConversationRole.User,
                    Content = [new ContentBlock { Text = prompt }]
                }
            ]
        };

        var response = await _client.ConverseAsync(converseRequest);

        var text = response.Output.Message.Content
            .FirstOrDefault(b => b.Text != null)?.Text ?? "{}";

        return JsonSerializer.Deserialize<InvestigationSummary>(text, _jsonOptions)
            ?? new InvestigationSummary();
    }
}
