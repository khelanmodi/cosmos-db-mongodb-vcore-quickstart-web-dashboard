namespace Microsoft.Learn.AzureCosmosDBMongoDBVCoreQuickstart.Web.Models;

internal sealed record Settings
{
    public Uri? ApiRootEndpoint { get; init; }

    public required string StatusEndpoint { get; init; }

    public required string RetrieveEndpoint { get; init; }

    public required string UpsertEndpoint { get; init; }

    public required string DeleteEndpoint { get; init; }

    public required bool ShowEndpoint { get; init; }

    public required string HeaderSuffix { get; init; }
}