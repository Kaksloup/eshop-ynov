using BuildingBlocks.CQRS;
using Discount.Grpc.DTOs;

namespace Discount.Grpc.Features.Coupons.Queries.GetCouponById;

/// <summary>
/// Query to get a coupon by ID.
/// </summary>
public record GetCouponByIdQuery(int Id) : IQuery<CouponDto>;
