using Catalog.API.Models;

namespace Catalog.API.Features.Products.Queries.GetProductWithDiscountById;

/// <summary>
/// Represents the result of the <see cref="GetProductWithDiscountByIdQuery"/>.
/// Contains the product data including its associated discount information from the Discount Service.
/// </summary>
/// <param name="Product">The product with discount information.</param>
public record GetProductWithDiscountByIdQueryResult(Product Product);
