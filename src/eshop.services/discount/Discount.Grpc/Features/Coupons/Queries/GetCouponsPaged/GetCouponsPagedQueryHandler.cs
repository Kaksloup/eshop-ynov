using BuildingBlocks.CQRS;
using BuildingBlocks.Pagination;
using Discount.Grpc.Data.Repositories;
using Discount.Grpc.DTOs;

namespace Discount.Grpc.Features.Coupons.Queries.GetCouponsPaged;

/// <summary>
/// Handler for GetCouponsPagedQuery.
/// </summary>
public class GetCouponsPagedQueryHandler(IDiscountRepository repository)
    : IQueryHandler<GetCouponsPagedQuery, PaginatedResult<CouponDto>>
{
    public async Task<PaginatedResult<CouponDto>> Handle(GetCouponsPagedQuery query, CancellationToken cancellationToken)
    {
        var filter = query.Filter;

        var (items, totalCount) = await repository.GetPagedAsync(
            filter.Status,
            filter.ProductName,
            filter.CouponCode,
            filter.IsActive,
            filter.PageNumber,
            filter.PageSize,
            cancellationToken);

        var couponDtos = items.Select(MapToCouponDto).ToList();

        return new PaginatedResult<CouponDto>(
            filter.PageNumber,
            filter.PageSize,
            totalCount,
            couponDtos);
    }

    private static CouponDto MapToCouponDto(Models.Coupon coupon)
    {
        return new CouponDto
        {
            Id = coupon.Id,
            ProductName = coupon.ProductName,
            Description = coupon.Description,
            DiscountType = coupon.DiscountType,
            Percentage = coupon.Percentage,
            Amount = coupon.Amount,
            CouponCode = coupon.CouponCode,
            IsActive = coupon.IsActive,
            IsStackable = coupon.IsStackable,
            MaxStackablePercentage = coupon.MaxStackablePercentage,
            Status = coupon.Status,
            EffectiveStatus = coupon.GetEffectiveStatus(),
            StartDate = coupon.StartDate,
            EndDate = coupon.EndDate,
            MinimumPurchaseAmount = coupon.MinimumPurchaseAmount,
            CreatedAt = coupon.CreatedAt,
            UpdatedAt = coupon.UpdatedAt
        };
    }
}
