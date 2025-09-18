using BuildingBlocks.CQRS;
using Catalog.API.Models;

namespace Catalog.API.Features.Products.Queries.GetProductById;

/// <summary>
/// Represents a query to retrieve a product by its unique identifier.
/// This query returns a result of type <see cref="GetProductByIdQueryResult"/>.
/// </summary>
public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdQueryResult>;