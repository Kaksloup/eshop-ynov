using Catalog.API.Models;
using BuildingBlocks.Pagination;

namespace Catalog.API.Features.Products.Queries.GetProductByCategory;

/// <summary>
/// Represents the paginated result of a query to retrieve products by category.
/// Contains the retrieved products and pagination metadata.
/// </summary>
public record GetProductByCategoryQueryResult(PaginatedResult<Product> Result);
