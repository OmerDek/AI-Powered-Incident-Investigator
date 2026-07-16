using Amazon.S3;
using Amazon.S3.Model;

namespace AIIncidentInvestigator.Services;

/// <summary>
/// Real ILogFileService backed by S3. Swap for MockLogFileService in Program.cs once
/// S3:BucketName (appsettings.json) and AWS credentials are configured - nothing in the
/// UI needs to change, same as the other Mock/Real service pairs.
///
/// Assumes log files are stored under a "{agentName}/" key prefix in the bucket, e.g.
/// "Omer Dekel/DiscoveryAgent.log". Adjust the prefix convention below if the real
/// bucket layout differs (by node ID, by date, etc.).
/// </summary>
public class S3LogFileService : ILogFileService
{
    private readonly IAmazonS3 _s3;
    private readonly string _bucketName;

    public S3LogFileService(IAmazonS3 s3, IConfiguration configuration)
    {
        _s3 = s3;
        _bucketName = configuration["S3:BucketName"]
            ?? throw new InvalidOperationException("S3:BucketName is not configured in appsettings.json");
    }

    public async Task<List<string>> GetAvailableLogFilesAsync(string agentName)
    {
        var prefix = $"{agentName}/";
        var request = new ListObjectsV2Request
        {
            BucketName = _bucketName,
            Prefix = prefix
        };

        var fileNames = new List<string>();
        ListObjectsV2Response response;
        do
        {
            response = await _s3.ListObjectsV2Async(request);
            fileNames.AddRange((response.S3Objects ?? new List<S3Object>())
                .Select(o => o.Key[prefix.Length..])
                .Where(name => !string.IsNullOrEmpty(name)));
            request.ContinuationToken = response.NextContinuationToken;
        }
        while (response.IsTruncated == true);

        return fileNames;
    }

    public async Task<string> GetLogFileContentAsync(string agentName, string fileName)
    {
        var key = $"{agentName}/{fileName}";
        using var response = await _s3.GetObjectAsync(_bucketName, key);
        using var reader = new StreamReader(response.ResponseStream);
        return await reader.ReadToEndAsync();
    }

    public async Task<bool> HasLogsAsync(string agentName)
    {
        var response = await _s3.ListObjectsV2Async(new ListObjectsV2Request
        {
            BucketName = _bucketName,
            Prefix = $"{agentName}/",
            MaxKeys = 1
        });

        return response.S3Objects is { Count: > 0 };
    }
}
