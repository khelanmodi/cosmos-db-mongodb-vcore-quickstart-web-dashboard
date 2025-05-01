using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace Microsoft.Learn.AzureCosmosDBMongoDBVCoreQuickstart.Web.Services;

internal sealed class MongoDataService(
    ILogger<MongoDataService> logger,
    IOptions<Settings> settingsOptions,
    HttpClient httpClient
) : IDataService
{
    private readonly Settings settings = settingsOptions.Value;

    private bool clientConfigured = false;

    private HttpClient Client
    {
        get
        {
            if (!clientConfigured)
            {
                if (settings.ApiRootEndpoint is null)
                {
                    throw new InvalidOperationException("Base address is not set.");
                }
                httpClient.BaseAddress = settings.ApiRootEndpoint;
                clientConfigured = true;
            }
            return httpClient;
        }
    }

    public async Task<(bool, string)> ConnectAsync(CancellationToken cancellationToken = default)
    {
        Status? status;
        try
        {
            status = await Client.GetFromJsonAsync<Status>(settings.StatusEndpoint, cancellationToken);
        }
        catch (Exception ex) when (ex is HttpRequestException or SocketException or AggregateException)
        {
            logger.LogHttpError(ex.Message);
            return (false, string.Empty);
        }

        logger.LogConnected(status?.IsHealthy ?? false);

        return status switch
        {
            null => (false, string.Empty),
            { IsHealthy: true } => (true, status.Host),
            _ => (false, string.Empty)
        };
    }

    public async IAsyncEnumerable<Product> GetProductsAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        IAsyncEnumerable<Product?> products = Client.GetFromJsonAsAsyncEnumerable<Product?>(settings.RetrieveEndpoint, cancellationToken);

        logger.LogRetrievingDocuments();

        await foreach (Product? product in products)
        {
            if (product is not null)
            {
                yield return product;
            }
        }
    }

    public void Dispose() => httpClient.Dispose();
    public async Task<bool> UpsertProductAsync(Product product, CancellationToken cancellationToken)
    {
        HttpResponseMessage response = await Client.PostAsJsonAsync(settings.UpsertEndpoint, product, cancellationToken);

        logger.LogUpsertingDocument($"{product}");

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteProductAsync(string id, CancellationToken cancellationToken)
    {
        string endpoint = Path.Combine(settings.DeleteEndpoint, id);

        HttpResponseMessage response = await Client.DeleteAsync(endpoint, cancellationToken);

        logger.LogDeletingDocument(id);

        return response.IsSuccessStatusCode;
    }
}

internal static partial class Logging
{
    [LoggerMessage(0, LogLevel.Information, "Connected to data service: {Connected}", EventName = "Running")]
    public static partial void LogConnected(this ILogger logger, bool connected);

    [LoggerMessage(1, LogLevel.Error, "Error connecting to the data service: {Message}", EventName = "HttpError")]
    public static partial void LogHttpError(this ILogger logger, string message);

    [LoggerMessage(2, LogLevel.Information, "Retrieving documents from MongoDB", EventName = "RetrievingDocuments")]
    public static partial void LogRetrievingDocuments(this ILogger logger);

    [LoggerMessage(3, LogLevel.Information, "Upserting document to MongoDB: {Document}", EventName = "UpsertingDocument")]
    public static partial void LogUpsertingDocument(this ILogger logger, string document);

    [LoggerMessage(4, LogLevel.Information, "Deleting document from MongoDB: {Id}", EventName = "DeletingDocument")]
    public static partial void LogDeletingDocument(this ILogger logger, string id);
}