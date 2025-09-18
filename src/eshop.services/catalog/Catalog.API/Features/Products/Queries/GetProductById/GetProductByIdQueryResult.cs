using Catalog.API.Models;

namespace Catalog.API.Features.Products.Queries.GetProductById;

/// <summary>
/// Represents the result of a query to retrieve a product by its unique identifier.
/// Contains the retrieved <see cref="Product"/> details.
/// </summary>
public record GetProductByIdQueryResult(Product Product);