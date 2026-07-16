using NodeWatch.AIInsights.Api.Providers;
using NodeWatch.AIInsights.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ILLMProvider, BedrockProvider>();
builder.Services.AddScoped<IPromptBuilderService, PromptBuilderService>();
builder.Services.AddScoped<IAiAnalysisService, AiAnalysisService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
