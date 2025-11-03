using BuildingBlocks.CQRS;
using Discount.Grpc.Data.Repositories;
using Discount.Grpc.DTOs;

namespace Discount.Grpc.Features.Coupons.Commands.UpdateCoupon;

/// <summary>
/// Handler for UpdateCouponCommand.
/// </summary>
public class UpdateCouponCommandHandler(IDiscountRepository repository)
    : ICommandHandler<UpdateCouponCommand, CouponDto>
{
    public async Task<CouponDto> Handle(UpdateCouponCommand command, CancellationToken cancellationToken)
    {
        var existingCoupon = await repository.GetByIdAsync(command.Id, cancellationToken);
        
        if (existingCoupon == null)
        {
            throw new KeyNotFoundException($"Coupon with ID {command.Id} not found");
        }

        // Update only provided fields
        if (command.Coupon.ProductName != null)
            existingCoupon.ProductName = command.Coupon.ProductName;
        
        if (command.Coupon.Description != null)
            existingCoupon.Description = command.Coupon.Description;
        
        if (command.Coupon.DiscountType.HasValue)
            existingCoupon.DiscountType = command.Coupon.DiscountType.Value;
        
        if (command.Coupon.Percentage.HasValue)
            existingCoupon.Percentage = command.Coupon.Percentage.Value;
        
        if (command.Coupon.Amount.HasValue)
            existingCoupon.Amount = command.Coupon.Amount.Value;
        
        if (command.Coupon.CouponCode != null)
            existingCoupon.CouponCode = command.Coupon.CouponCode;
        
        if (command.Coupon.IsStackable.HasValue)
            existingCoupon.IsStackable = command.Coupon.IsStackable.Value;
        
        if (command.Coupon.MaxStackablePercentage.HasValue)
            existingCoupon.MaxStackablePercentage = command.Coupon.MaxStackablePercentage.Value;
        
        if (command.Coupon.StartDate.HasValue)
            existingCoupon.StartDate = command.Coupon.StartDate.Value;
        
        if (command.Coupon.EndDate.HasValue)
            existingCoupon.EndDate = command.Coupon.EndDate.Value;
        
        if (command.Coupon.MinimumPurchaseAmount.HasValue)
            existingCoupon.MinimumPurchaseAmount = command.Coupon.MinimumPurchaseAmount.Value;
        
        if (command.Coupon.IsActive.HasValue)
            existingCoupon.IsActive = command.Coupon.IsActive.Value;

        existingCoupon.UpdatedAt = DateTime.UtcNow;
        existingCoupon.Status = existingCoupon.GetEffectiveStatus();

        var updated = await repository.UpdateAsync(existingCoupon, cancellationToken);

        return MapToCouponDto(updated);
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
