using BuildingBlocks.CQRS;
using Discount.Grpc.Data.Repositories;

namespace Discount.Grpc.Features.Discounts.Commands.DeleteDiscount;

/// <summary>
/// Handler for deleting a discount coupon.
/// </summary>
public class DeleteDiscountCommandHandler(
    IDiscountRepository repository,
    ILogger<DeleteDiscountCommandHandler> logger)
    : ICommandHandler<DeleteDiscountCommand, DeleteDiscountCommandResult>
{
    public async Task<DeleteDiscountCommandResult> Handle(
        DeleteDiscountCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting discount for product: {ProductName}", request.ProductName);

        // Find the coupon by product name or ID
        var coupon = await repository.GetByProductNameAsync(request.ProductName, cancellationToken)
            ?? await repository.GetByIdAsync(request.Id, cancellationToken);

        if (coupon is null)
        {
            throw new InvalidOperationException(
                $"Coupon with name {request.ProductName} or Id {request.Id} not found");
        }

        var success = await repository.DeleteAsync(coupon, cancellationToken);

        logger.LogInformation("Discount deleted for {ProductName}", coupon.ProductName);

        return new DeleteDiscountCommandResult(success);
    }
}
