using Discount.Grpc.Features.Discounts.Commands.CreateDiscount;
using Discount.Grpc.Features.Discounts.Commands.DeleteDiscount;
using Discount.Grpc.Features.Discounts.Commands.UpdateDiscount;
using Discount.Grpc.Features.Discounts.Queries.GetDiscount;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using MediatR;

namespace Discount.Grpc.Services;

/// <summary>
/// The DiscountServiceServer class implements the gRPC service for managing discount data.
/// It provides CRUD operations for discounts using CQRS pattern with MediatR.
/// This class inherits from DiscountProtoServiceBase and orchestrates commands and queries
/// through the MediatR pipeline.
/// </summary>
/// <remarks>
/// This class uses MediatR (ISender) to dispatch commands and queries to their respective handlers,
/// following the CQRS architectural pattern for better separation of concerns and testability.
/// </remarks>
public class DiscountServiceServer(ISender sender, ILogger<DiscountServiceServer> logger) : DiscountProtoService.DiscountProtoServiceBase
{
    /// <summary>
    /// Retrieves discount details for a given product from the database.
    /// </summary>
    /// <param name="request">The request containing the product name to fetch the discount for.</param>
    /// <param name="context">The gRPC server call context.</param>
    /// <returns>
    /// Returns a <see cref="CouponModel"/> containing the discount details for the specified product.
    /// </returns>
    /// <exception cref="RpcException">
    /// Thrown if no discount is found for the specified product name.
    /// </exception>
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        try
        {
            var query = new GetDiscountQuery(request.ProductName);
            var result = await sender.Send(query, context.CancellationToken);
            
            return result.Coupon.Adapt<CouponModel>();
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Discount not found for {ProductName}", request.ProductName);
            throw new RpcException(new Status(StatusCode.NotFound, ex.Message));
        }
    }

    /// <summary>
    /// Creates a new discount for a specified product and stores it in the database.
    /// </summary>
    /// <param name="request">The request containing the details of the new discount to create, including the coupon information.</param>
    /// <param name="context">The gRPC server call context.</param>
    /// <returns>
    /// Returns a <see cref="CouponModel"/> representing the newly created discount.
    /// </returns>
    /// <exception cref="RpcException">
    /// Thrown if the request's coupon information is null.
    /// </exception>
    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        if (request.Coupon is null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Coupon is null"));
        
        var coupon = request.Coupon.Adapt<Coupon>();
        var command = new CreateDiscountCommand(coupon);
        var result = await sender.Send(command, context.CancellationToken);
        
        return result.Coupon.Adapt<CouponModel>();
    }

    /// <summary>
    /// Updates the discount details for a specific product based on the provided request.
    /// </summary>
    /// <param name="request">An object containing the updated discount information for a specific product.</param>
    /// <param name="context">The gRPC server call context.</param>
    /// <returns>
    /// Returns an updated <see cref="CouponModel"/> containing the modified discount details.
    /// </returns>
    /// <exception cref="RpcException">
    /// Thrown if the provided coupon is null, or if the specified product or coupon identifier is not found in the database.
    /// </exception>
    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        if (request.Coupon is null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Coupon is null"));
        
        try
        {
            var coupon = request.Coupon.Adapt<Coupon>();
            var command = new UpdateDiscountCommand(coupon);
            var result = await sender.Send(command, context.CancellationToken);
            
            return result.Coupon.Adapt<CouponModel>();
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Coupon not found for update");
            throw new RpcException(new Status(StatusCode.NotFound, ex.Message));
        }
    }

    /// <summary>
    /// Deletes a discount for a specified product based on the provided coupon details.
    /// </summary>
    /// <param name="request">The request containing the details of the coupon to be deleted, including the product name or ID.</param>
    /// <param name="context">The gRPC server call context.</param>
    /// <returns>
    /// Returns a <see cref="DeleteDiscountResponse"/> indicating whether the discount was successfully deleted.
    /// </returns>
    /// <exception cref="RpcException">
    /// Thrown if the provided coupon is null, or if no matching discount is found for the specified product name or ID.
    /// </exception>
    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request,
        ServerCallContext context)
    {
        if (request.Coupon is null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Coupon is null"));

        try
        {
            var command = new DeleteDiscountCommand(request.Coupon.ProductName, request.Coupon.Id);
            var result = await sender.Send(command, context.CancellationToken);
            
            return new DeleteDiscountResponse { Success = result.Success };
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Coupon not found for deletion");
            throw new RpcException(new Status(StatusCode.NotFound, ex.Message));
        }
    }
}