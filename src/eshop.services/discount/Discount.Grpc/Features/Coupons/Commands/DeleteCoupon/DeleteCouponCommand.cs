using BuildingBlocks.CQRS;

namespace Discount.Grpc.Features.Coupons.Commands.DeleteCoupon;

/// <summary>
/// Command to delete a coupon.
/// </summary>
public record DeleteCouponCommand(int Id) : ICommand<bool>;
