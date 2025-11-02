using BuildingBlocks.CQRS;
using Discount.Grpc.Models;

namespace Discount.Grpc.Features.Discounts.Queries.GetDiscount;

/// <summary>
/// Query to retrieve a discount coupon by product name.
/// </summary>
/// <param name="ProductName">The name of the product to get the discount for.</param>
public record GetDiscountQuery(string ProductName) : IQuery<GetDiscountQueryResult>;

/// <summary>
/// Result containing the discount coupon information.
/// </summary>
/// <param name="Coupon">The discount coupon.</param>
public record GetDiscountQueryResult(Coupon Coupon);
