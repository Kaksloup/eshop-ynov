using BuildingBlocks.CQRS;
using BuildingBlocks.Pagination;
using Discount.Grpc.DTOs;

namespace Discount.Grpc.Features.Coupons.Queries.GetCouponsPaged;

/// <summary>
/// Query to get paginated and filtered list of coupons.
/// </summary>
public record GetCouponsPagedQuery(CouponFilterDto Filter) : IQuery<PaginatedResult<CouponDto>>;
