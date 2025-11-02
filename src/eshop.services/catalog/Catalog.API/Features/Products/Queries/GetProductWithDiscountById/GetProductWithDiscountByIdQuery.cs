using BuildingBlocks.CQRS;
using Catalog.API.Models;

namespace Catalog.API.Features.Products.Queries.GetProductWithDiscountById;

/// <summary>
/// Represents a query to retrieve a product by its unique identifier with its associated discount information.
/// This query returns a result of type <see cref="GetProductWithDiscountByIdQueryResult"/>.
/// The discount information is fetched from the Discount Service via gRPC.
/// </summary>
public record GetProductWithDiscountByIdQuery(Guid Id) : IQuery<GetProductWithDiscountByIdQueryResult>;
