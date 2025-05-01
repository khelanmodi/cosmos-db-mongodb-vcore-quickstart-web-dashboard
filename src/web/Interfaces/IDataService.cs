namespace Microsoft.Learn.AzureCosmosDBMongoDBVCoreQuickstart.Web.Interfaces;

internal interface IDataService : IDisposable
{
    Task<(bool success, string endpoint)> ConnectAsync(CancellationToken cancellationToken);

    IAsyncEnumerable<Product> GetProductsAsync(CancellationToken cancellationToken);

    Task<bool> UpsertProductAsync(Product product, CancellationToken cancellationToken);

    Task<bool> DeleteProductAsync(string id, CancellationToken cancellationToken);
}