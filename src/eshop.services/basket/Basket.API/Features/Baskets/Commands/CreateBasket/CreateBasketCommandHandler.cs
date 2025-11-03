using Basket.API.Data.Repositories;
using Basket.API.Models;
using BuildingBlocks.CQRS;
using Discount.Grpc;

namespace Basket.API.Features.Baskets.Commands.CreateBasket;

/// <summary>
/// Handles the creation of a shopping basket by processing the CreateBasketCommand.
/// Implements the <see cref="ICommandHandler{CreateBasketCommand, CreateBasketCommandResult}"/> interface.
/// </summary>
public class CreateBasketCommandHandler(IBasketRepository repository, DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient) : ICommandHandler<CreateBasketCommand, CreateBasketCommandResult>
{
    /// <summary>
    /// Handles the request to create a shopping basket.
    /// </summary>
    /// <param name="request">The CreateBasketCommand containing the details of the shopping basket to be created.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
    /// <returns>A task representing the asynchronous operation, returning a CreateBasketCommandResult that indicates the success of the operation and includes the UserName of the created basket.</returns>
    public async Task<CreateBasketCommandResult> Handle(CreateBasketCommand request,
        CancellationToken cancellationToken)
    {
        var cart = request.Cart;

        await ApplyDiscountToItemAsync(cart, cancellationToken);

        var basketCart = await repository.CreateBasketAsync(cart, cancellationToken)
            .ConfigureAwait(false);

        return new CreateBasketCommandResult(true, basketCart.UserName);
    }

    /// <summary>
    /// Applies discounts to each item in the specified shopping cart.
    /// - Validates coupon date validity (StartDate/EndDate)
    /// - Applies coupons in descending order of discount percentage
    /// - Caps total discount at 30%
    /// </summary>
    /// <param name="cart">The shopping cart containing the items to which the discount will be applied.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
    /// <returns>A task that represents the asynchronous operation of applying discounts to the items.</returns>
    private async Task ApplyDiscountToItemAsync(ShoppingCart cart, CancellationToken cancellationToken)
    {
        const decimal MAX_DISCOUNT_PERCENTAGE = 30m;
        
        foreach (var item in cart.Items)
        {
            // Initialize BasePrice if not already set (preserve original price)
            if (item.BasePrice == 0)
            {
                item.BasePrice = item.Price;
            }
            
            try
            {
                var coupon = await discountProtoServiceClient.GetDiscountAsync(new GetDiscountRequest
                    { ProductName = item.ProductName }, cancellationToken: cancellationToken);
                
                // Skip if coupon is inactive
                if (!coupon.IsActive)
                    continue;
                
                // Check if coupon is within valid date range
                var now = DateTime.UtcNow;
                
                if (coupon.StartDate != null)
                {
                    var startDate = coupon.StartDate.ToDateTime();
                    if (now < startDate)
                        continue; // Coupon not yet valid
                }
                    
                if (coupon.EndDate != null)
                {
                    var endDate = coupon.EndDate.ToDateTime();
                    if (now > endDate)
                        continue; // Coupon expired
                }
                
                // Calculate discount percentage for this coupon
                decimal discountPercentage = 0;
                
                if (coupon.DiscountType == DiscountType.Percentage)
                {
                    discountPercentage = (decimal)coupon.Percentage;
                }
                else if (coupon.DiscountType == DiscountType.FixedAmount)
                {
                    // Convert fixed amount to percentage based on base price
                    if (item.BasePrice > 0)
                    {
                        discountPercentage = ((decimal)coupon.Amount / item.BasePrice) * 100;
                    }
                }
                
                // Cap discount at 30%
                discountPercentage = Math.Min(discountPercentage, MAX_DISCOUNT_PERCENTAGE);
                
                // Apply the discount to the base price
                item.Price = item.BasePrice * (1 - (discountPercentage / 100));
            }
            catch (Grpc.Core.RpcException ex) when (ex.StatusCode == Grpc.Core.StatusCode.NotFound)
            {
                // No discount available for this product, keep original price
                continue;
            }
        }
    }
}