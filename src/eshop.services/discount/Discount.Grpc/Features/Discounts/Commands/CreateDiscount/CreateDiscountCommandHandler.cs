using BuildingBlocks.CQRS;
using Discount.Grpc.Business;
using Discount.Grpc.Data.Repositories;

namespace Discount.Grpc.Features.Discounts.Commands.CreateDiscount;

/// <summary>
/// Handler for creating a new discount coupon.
/// </summary>
public class CreateDiscountCommandHandler(
    IDiscountRepository repository,
    ILogger<CreateDiscountCommandHandler> logger)
    : ICommandHandler<CreateDiscountCommand, CreateDiscountCommandResult>
{
    public async Task<CreateDiscountCommandResult> Handle(
        CreateDiscountCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating discount for product: {ProductName}", request.Coupon.ProductName);

        // Get existing active coupons for the same product to validate stacking rules
        var existingCoupons = await repository.GetActiveByProductNameAsync(
            request.Coupon.ProductName, 
            cancellationToken);

        // Validate discount against business rules
        var validationResult = DiscountRulesEngine.ValidateDiscount(request.Coupon, existingCoupons);
        
        if (!validationResult.IsValid)
        {
            logger.LogWarning("Discount validation failed for {ProductName}: {Error}", 
                request.Coupon.ProductName, validationResult.ErrorMessage);
            throw new InvalidOperationException(validationResult.ErrorMessage);
        }

        var createdCoupon = await repository.CreateAsync(request.Coupon, cancellationToken);

        logger.LogInformation("Discount created for {ProductName}: {Percentage}%",
            createdCoupon.ProductName, createdCoupon.Percentage);

        return new CreateDiscountCommandResult(createdCoupon);
    }
}
