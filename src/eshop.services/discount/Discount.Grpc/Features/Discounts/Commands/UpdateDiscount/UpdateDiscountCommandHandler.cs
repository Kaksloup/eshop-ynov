using BuildingBlocks.CQRS;
using Discount.Grpc.Business;
using Discount.Grpc.Data.Repositories;

namespace Discount.Grpc.Features.Discounts.Commands.UpdateDiscount;

/// <summary>
/// Handler for updating an existing discount coupon.
/// </summary>
public class UpdateDiscountCommandHandler(
    IDiscountRepository repository,
    ILogger<UpdateDiscountCommandHandler> logger)
    : ICommandHandler<UpdateDiscountCommand, UpdateDiscountCommandResult>
{
    public async Task<UpdateDiscountCommandResult> Handle(
        UpdateDiscountCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating discount for product: {ProductName}", request.Coupon.ProductName);

        // Verify the coupon exists
        var existingCoupon = await repository.GetByProductNameAsync(request.Coupon.ProductName, cancellationToken)
            ?? await repository.GetByIdAsync(request.Coupon.Id, cancellationToken);

        if (existingCoupon is null)
        {
            throw new InvalidOperationException(
                $"Coupon with name {request.Coupon.ProductName} or Id {request.Coupon.Id} not found");
        }

        // Get other active coupons for the same product (excluding the one being updated)
        var otherCoupons = await repository.GetActiveByProductNameAsync(
            request.Coupon.ProductName, 
            cancellationToken);
        otherCoupons.RemoveAll(c => c.Id == request.Coupon.Id);

        // Validate the updated discount against business rules
        var validationResult = DiscountRulesEngine.ValidateDiscount(request.Coupon, otherCoupons);
        
        if (!validationResult.IsValid)
        {
            logger.LogWarning("Discount validation failed for {ProductName}: {Error}", 
                request.Coupon.ProductName, validationResult.ErrorMessage);
            throw new InvalidOperationException(validationResult.ErrorMessage);
        }

        // Update the existing coupon with new values
        existingCoupon.ProductName = request.Coupon.ProductName;
        existingCoupon.Description = request.Coupon.Description;
        existingCoupon.Percentage = request.Coupon.Percentage;
        existingCoupon.Amount = request.Coupon.Amount;
        existingCoupon.DiscountType = request.Coupon.DiscountType;
        existingCoupon.IsStackable = request.Coupon.IsStackable;
        existingCoupon.IsActive = request.Coupon.IsActive;

        var updatedCoupon = await repository.UpdateAsync(existingCoupon, cancellationToken);

        logger.LogInformation("Discount updated for {ProductName}: {Percentage}%",
            updatedCoupon.ProductName, updatedCoupon.Percentage);

        return new UpdateDiscountCommandResult(updatedCoupon);
    }
}
