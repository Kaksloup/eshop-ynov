using BuildingBlocks.CQRS;
using Catalog.API.Models;

namespace Catalog.API.Features.Products.Queries.GetProductByCategory;

/// <summary>
/// Represents a query to retrieve a product by its unique identifier.
/// This query returns a result of type <see cref="GetProductByCategoryQueryResult"/>.
/// </summary>
public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryQueryResult>;