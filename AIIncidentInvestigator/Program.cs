using AIIncidentInvestigator.Components;
using AIIncidentInvestigator.Services;
using Amazon.S3;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

var assignedPort = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrWhiteSpace(assignedPort))
{
    builder.WebHost.UseUrls($"http://localhost:{assignedPort}");
}

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();
builder.Services.AddHttpClient();

// AWS S3 - reads the "AWS" section from appsettings.json (region, credentials via env/profile).
// Local dev credentials (dotnet user-secrets "AWS:AccessKey"/"AWS:SecretKey") are wired in explicitly
// here because the SDK's default credential chain doesn't check .NET user-secrets on its own -
// it only checks env vars, ~/.aws/credentials, or an EC2 instance role.
var awsOptions = builder.Configuration.GetAWSOptions();
var awsAccessKey = builder.Configuration["AWS:AccessKey"];
var awsSecretKey = builder.Configuration["AWS:SecretKey"];
if (!string.IsNullOrWhiteSpace(awsAccessKey) && !string.IsNullOrWhiteSpace(awsSecretKey))
{
    awsOptions.Credentials = new Amazon.Runtime.BasicAWSCredentials(awsAccessKey, awsSecretKey);
}
builder.Services.AddDefaultAWSOptions(awsOptions);
builder.Services.AddAWSService<IAmazonS3>();

builder.Services.AddScoped<ILogFileService, S3LogFileService>();

// Swap to a real Node Watch fleet data source once available.
builder.Services.AddScoped<IAgentDirectoryService, MockAgentDirectoryService>();

builder.Services.AddScoped<IAiAnalysisClient, RealAiAnalysisClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Backend API surface per backend.md - the Blazor UI in this project calls these
// services in-process, but the endpoints exist so Node Watch (or any other client)
// can reach the same data over HTTP.
app.MapGet("/api/agents", async (IAgentDirectoryService agents) =>
    await agents.GetAgentsAsync());

app.MapGet("/api/agents/{agentName}", async (string agentName, IAgentDirectoryService agents) =>
{
    var agent = await agents.GetAgentAsync(agentName);
    return agent is null ? Results.NotFound() : Results.Ok(agent);
});

app.MapGet("/api/agents/{agentName}/logs", async (string agentName, ILogFileService logFiles) =>
    Results.Ok(new { logFiles = await logFiles.GetAvailableLogFilesAsync(agentName) }));

// backend.md documents this as GET /api/logs/{fileName}; agentName is added to the route
// since a bare file name doesn't identify which agent's logs to read.
app.MapGet("/api/agents/{agentName}/logs/{fileName}", async (string agentName, string fileName, ILogFileService logFiles) =>
{
    var content = await logFiles.GetLogFileContentAsync(agentName, fileName);
    return Results.Ok(new { fileName, content });
});

app.Run();
