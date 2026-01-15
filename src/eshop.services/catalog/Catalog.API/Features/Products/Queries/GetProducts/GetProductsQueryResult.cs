using BuildingBlocks.Pagination;
using Catalog.API.Models;

namespace Catalog.API.Features.Products.Queries.GetProducts;

/// <summary>
/// Represents the result of a query to retrieve a list of products.
/// Contains the retrieved products and pagination metadata.
/// </summary>
public record GetProductsQueryResult(PaginatedResult<Product> Result);