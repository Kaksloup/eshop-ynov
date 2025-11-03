using BuildingBlocks.CQRS;
using Discount.Grpc.Data.Repositories;
using Discount.Grpc.DTOs;

namespace Discount.Grpc.Features.Coupons.Queries.GetCouponById;

/// <summary>
/// Handler for GetCouponByIdQuery.
/// </summary>
public class GetCouponByIdQueryHandler(IDiscountRepository repository)
    : IQueryHandler<GetCouponByIdQuery, CouponDto>
{
    public async Task<CouponDto> Handle(GetCouponByIdQuery query, CancellationToken cancellationToken)
    {
        var coupon = await repository.GetByIdAsync(query.Id, cancellationToken);
        
        if (coupon == null)
        {
            throw new KeyNotFoundException($"Coupon with ID {query.Id} not found");
        }

        return MapToCouponDto(coupon);
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
