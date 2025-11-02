using BuildingBlocks.CQRS;
using Discount.Grpc.Models;

namespace Discount.Grpc.Features.Discounts.Commands.UpdateDiscount;

/// <summary>
/// Command to update an existing discount coupon.
/// </summary>
/// <param name="Coupon">The coupon information to update.</param>
public record UpdateDiscountCommand(Coupon Coupon) : ICommand<UpdateDiscountCommandResult>;

/// <summary>
/// Result containing the updated discount coupon.
/// </summary>
/// <param name="Coupon">The updated coupon.</param>
public record UpdateDiscountCommandResult(Coupon Coupon);
