namespace Microsoft.Learn.AzureCosmosDBMongoDBVCoreQuickstart.Web.Models;

internal sealed record Status
{
    public required string Host { get; init; }

    public required bool IsHealthy { get; init; }
}