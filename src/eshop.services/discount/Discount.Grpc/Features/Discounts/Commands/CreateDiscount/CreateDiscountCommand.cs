using BuildingBlocks.CQRS;
using Discount.Grpc.Models;

namespace Discount.Grpc.Features.Discounts.Commands.CreateDiscount;

/// <summary>
/// Command to create a new discount coupon.
/// </summary>
/// <param name="Coupon">The coupon information to create.</param>
public record CreateDiscountCommand(Coupon Coupon) : ICommand<CreateDiscountCommandResult>;

/// <summary>
/// Result containing the created discount coupon.
/// </summary>
/// <param name="Coupon">The created coupon.</param>
public record CreateDiscountCommandResult(Coupon Coupon);
