using BuildingBlocks.CQRS;
using Discount.Grpc.DTOs;

namespace Discount.Grpc.Features.Coupons.Commands.UpdateCoupon;

/// <summary>
/// Command to update an existing coupon.
/// </summary>
public record UpdateCouponCommand(int Id, UpdateCouponDto Coupon) : ICommand<CouponDto>;
