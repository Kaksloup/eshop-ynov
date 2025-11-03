using BuildingBlocks.CQRS;
using Discount.Grpc.DTOs;

namespace Discount.Grpc.Features.Coupons.Commands.CreateCoupon;

/// <summary>
/// Command to create a new coupon.
/// </summary>
public record CreateCouponCommand(CreateCouponDto Coupon) : ICommand<CouponDto>;
