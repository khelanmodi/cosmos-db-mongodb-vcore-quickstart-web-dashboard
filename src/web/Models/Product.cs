namespace Microsoft.Learn.AzureCosmosDBMongoDBVCoreQuickstart.Web.Models;

/// <summary>
/// Represents a product in the inventory.
/// </summary>
public sealed record Product
{
    /// <summary>
    /// Gets the unique identifier (id) for the product.
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    /// Gets the category of the product.
    /// </summary>
    public required string Category { get; set; }

    /// <summary>
    /// Gets the name of the product.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets the description of the product.
    /// </summary>
    public required int Quantity { get; set; }

    /// <summary>
    /// Gets the price of the product.
    /// </summary>
    public required decimal Price { get; set; }

    /// <summary>
    /// Gets the clearance status of the product.
    /// </summary>
    public required bool Clearance { get; set; }

    /// <summary>
    /// Generates a default product with blank values.
    /// </summary>
    public static Product GenerateDefaultProduct() => new()
    {
        Id = $"{Guid.NewGuid()}",
        Category = string.Empty,
        Name = string.Empty,
        Quantity = 0,
        Price = 0.0m,
        Clearance = false
    };
}