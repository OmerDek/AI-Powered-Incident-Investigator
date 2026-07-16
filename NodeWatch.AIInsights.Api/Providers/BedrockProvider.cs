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

        var hasBearerToken = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("AWS_BEARER_TOKEN_BEDROCK"));
        var hasAccessKey = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID"));

        if (!hasBearerToken && !hasAccessKey)
            throw new InvalidOperationException(
                "No AWS credentials configured. Set either AWS_BEARER_TOKEN_BEDROCK (short-lived Bedrock API key) " +
                "or AWS_ACCESS_KEY_ID/AWS_SECRET_ACCESS_KEY (standard IAM credentials) and restart the app.");

        // If AWS_BEARER_TOKEN_BEDROCK is set, the SDK auto-detects and signs requests with it.
        // Otherwise it falls back to the standard AWS credential chain (AWS_ACCESS_KEY_ID/
        // AWS_SECRET_ACCESS_KEY env vars, ~/.aws/credentials, or an instance role) automatically —
        // no explicit Credentials needed either way.
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
