using BuildingBlocks.CQRS;
using Discount.Grpc.Data.Repositories;

namespace Discount.Grpc.Features.Coupons.Commands.DeleteCoupon;

/// <summary>
/// Handler for DeleteCouponCommand.
/// </summary>
public class DeleteCouponCommandHandler(IDiscountRepository repository)
    : ICommandHandler<DeleteCouponCommand, bool>
{
    public async Task<bool> Handle(DeleteCouponCommand command, CancellationToken cancellationToken)
    {
        var existingCoupon = await repository.GetByIdAsync(command.Id, cancellationToken);
        
        if (existingCoupon == null)
        {
            throw new KeyNotFoundException($"Coupon with ID {command.Id} not found");
        }

        return await repository.DeleteAsync(existingCoupon, cancellationToken);
    }
}
