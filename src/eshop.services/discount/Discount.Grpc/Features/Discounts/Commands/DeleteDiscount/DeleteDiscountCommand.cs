using BuildingBlocks.CQRS;

namespace Discount.Grpc.Features.Discounts.Commands.DeleteDiscount;

/// <summary>
/// Command to delete a discount coupon.
/// </summary>
/// <param name="ProductName">The product name of the coupon to delete.</param>
/// <param name="Id">The ID of the coupon to delete.</param>
public record DeleteDiscountCommand(string ProductName, int Id) : ICommand<DeleteDiscountCommandResult>;

/// <summary>
/// Result indicating whether the deletion was successful.
/// </summary>
/// <param name="Success">True if deletion was successful.</param>
public record DeleteDiscountCommandResult(bool Success);
