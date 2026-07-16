# AI-Powered Incident Investigator

Two independently deployable services that together form the hackathon project (Node Watch AI Insights).

## Structure

- **`AIIncidentInvestigator/`** — Blazor Server app (frontend + backend). Node Watch fleet list, Troubleshoot Data drill-in, log preview, and the AI Investigation Summary UI. Reads logs from S3 and calls the AI Analyzer service below.
- **`NodeWatch.AIInsights.Api/`** — AI Analyzer API (ASP.NET Core Web API). Receives agent metadata + log file content, builds a prompt, and calls AWS Bedrock to generate a structured investigation summary.

Each has its own `.csproj`/`.sln` and is published independently (each as its own Azure App Service) — they only communicate over HTTP (`POST /api/analysis/analyze`).

## Running locally

Each project runs independently with `dotnet run` from its own folder. `AIIncidentInvestigator` needs AWS credentials (S3 + optionally Bedrock) supplied via `dotnet user-secrets` or environment variables — see `appsettings.json` in each project for the config keys expected (`AWS:*`, `S3:BucketName`, `AiService:BaseUrl`).
