using BuildingBlocks.CQRS;
using Discount.Grpc.Data.Repositories;

namespace Discount.Grpc.Features.Discounts.Queries.GetDiscount;

/// <summary>
/// Handler for retrieving a discount coupon by product name.
/// </summary>
public class GetDiscountQueryHandler(
    IDiscountRepository repository,
    ILogger<GetDiscountQueryHandler> logger)
    : IQueryHandler<GetDiscountQuery, GetDiscountQueryResult>
{
    public async Task<GetDiscountQueryResult> Handle(
        GetDiscountQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving discount for product: {ProductName}", request.ProductName);

        var coupon = await repository.GetByProductNameAsync(request.ProductName, cancellationToken);

        if (coupon is null)
        {
            logger.LogWarning("Discount not found for product: {ProductName}", request.ProductName);
            throw new InvalidOperationException($"Coupon with product name {request.ProductName} not found");
        }

        logger.LogInformation("Discount retrieved for {ProductName}: {Percentage}%",
            coupon.ProductName, coupon.Percentage);

        return new GetDiscountQueryResult(coupon);
    }
}
