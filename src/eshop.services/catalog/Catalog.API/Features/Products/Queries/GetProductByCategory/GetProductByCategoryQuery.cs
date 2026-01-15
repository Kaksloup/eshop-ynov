using BuildingBlocks.CQRS;

namespace Catalog.API.Features.Products.Queries.GetProductByCategory;

/// <summary>
/// Represents a query to retrieve products by category with pagination.
/// This query returns a paginated result of products (<see cref="GetProductByCategoryQueryResult"/>).
/// </summary>
public record GetProductByCategoryQuery(
    string Category,
    int PageNumber = 1,
    int PageSize = 10
) : IQuery<GetProductByCategoryQueryResult>;
