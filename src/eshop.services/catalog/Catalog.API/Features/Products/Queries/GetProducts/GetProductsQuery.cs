using BuildingBlocks.CQRS;

namespace Catalog.API.Features.Products.Queries.GetProducts;

/// <summary>
/// Represents a query to retrieve a list of products.
/// This query returns a result of type <see cref="GetProductsQueryResult"/>.
/// </summary>
public record GetProductsQuery(int PageNumber = 1, int PageSize = 10) : IQuery<GetProductsQueryResult>;