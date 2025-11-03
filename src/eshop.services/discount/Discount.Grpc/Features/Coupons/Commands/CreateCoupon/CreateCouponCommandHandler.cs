using BuildingBlocks.CQRS;
using Discount.Grpc.Data.Repositories;
using Discount.Grpc.DTOs;
using Discount.Grpc.Models;

namespace Discount.Grpc.Features.Coupons.Commands.CreateCoupon;

/// <summary>
/// Handler for CreateCouponCommand.
/// </summary>
public class CreateCouponCommandHandler(IDiscountRepository repository)
    : ICommandHandler<CreateCouponCommand, CouponDto>
{
    public async Task<CouponDto> Handle(CreateCouponCommand command, CancellationToken cancellationToken)
    {
        var coupon = new Coupon
        {
            ProductName = command.Coupon.ProductName,
            Description = command.Coupon.Description,
            DiscountType = command.Coupon.DiscountType,
            Percentage = command.Coupon.Percentage,
            Amount = command.Coupon.Amount,
            CouponCode = command.Coupon.CouponCode,
            IsStackable = command.Coupon.IsStackable,
            MaxStackablePercentage = command.Coupon.MaxStackablePercentage,
            StartDate = command.Coupon.StartDate,
            EndDate = command.Coupon.EndDate,
            MinimumPurchaseAmount = command.Coupon.MinimumPurchaseAmount,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        // Set initial status based on start date
        coupon.Status = coupon.GetEffectiveStatus();

        var created = await repository.CreateAsync(coupon, cancellationToken);

        return MapToCouponDto(created);
    }

    private static CouponDto MapToCouponDto(Coupon coupon)
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
