using BuildingBlocks.CQRS;
using Catalog.API.DTOs;
using Catalog.API.Exceptions;
using Catalog.API.Models;
using Discount.Grpc;
using Marten;
using Mapster;

namespace Catalog.API.Features.Products.Queries.GetProductWithDiscountById;

/// <summary>
/// Handles the execution of the <see cref="GetProductWithDiscountByIdQuery"/> and retrieves the corresponding
/// product data from the data store, enriched with discount information from the Discount Service via gRPC.
/// </summary>
/// <remarks>
/// This class interacts with the database session to load the product identified by
/// the specified <see cref="Guid"/> in the query. If the product does not exist,
/// a <see cref="ProductNotFoundException"/> is thrown.
/// 
/// Once the product is loaded, it calls the Discount Service to retrieve any active discount
/// information for that product. If a discount is found, it is mapped to the product object.
/// If no discount is found or an error occurs, the product is returned without discount information.
/// 
/// It utilizes a logger to log the status and outcome of the operation.
/// </remarks>
public class GetProductWithDiscountByIdQueryHandler(
    IDocumentSession documentSession,
    DiscountProtoService.DiscountProtoServiceClient discountClient,
    ILogger<GetProductWithDiscountByIdQueryHandler> logger)
    : IQueryHandler<GetProductWithDiscountByIdQuery, GetProductWithDiscountByIdQueryResult>
{
    /// <summary>
    /// Handles the execution of the GetProductWithDiscountByIdQuery and retrieves the associated product data
    /// enriched with discount information.
    /// </summary>
    /// <param name="request">The query containing the identifier for the product to be retrieved.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the result of the query, including the product data with discount information.</returns>
    /// <exception cref="ProductNotFoundException">Thrown when the product with the given identifier is not found in the database.</exception>
    public async Task<GetProductWithDiscountByIdQueryResult> Handle(
        GetProductWithDiscountByIdQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving product with ID: {ProductId}", request.Id);

        var product = await documentSession.LoadAsync<Product>(request.Id, cancellationToken);
        
        if (product is null)
        {
            logger.LogWarning("Product not found with ID: {ProductId}", request.Id);
            throw new ProductNotFoundException(request.Id);
        }

        // Try to retrieve discount information from the Discount Service
        try
        {
            logger.LogInformation("Fetching discount information for product: {ProductName}", product.Name);
            
            var discountRequest = new GetDiscountRequest { ProductName = product.Name };
            var discountResponse = await discountClient.GetDiscountAsync(discountRequest, cancellationToken: cancellationToken);
            
            // Map the gRPC coupon model to the DiscountInfoDto
            product.Discount = new DiscountInfoDto
            {
                Id = discountResponse.Id,
                ProductName = discountResponse.ProductName,
                Description = discountResponse.Description,
                DiscountType = (int)discountResponse.DiscountType,
                Percentage = discountResponse.Percentage,
                Amount = discountResponse.Amount,
                CouponCode = discountResponse.CouponCode,
                IsActive = discountResponse.IsActive,
                IsStackable = discountResponse.IsStackable
            };
            
            logger.LogInformation("Discount found for product {ProductName}: Type={DiscountType}, Percentage={Percentage}%, Amount={Amount}, Stackable={IsStackable}", 
                product.Name, product.Discount.DiscountType, product.Discount.Percentage, product.Discount.Amount, product.Discount.IsStackable);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Error retrieving discount information for product {ProductName}. " +
                "Continuing without discount.", product.Name);
            // In case of communication error with Discount service,
            // continue without discount information to not block product retrieval
            product.Discount = null;
        }

        logger.LogInformation("Successfully retrieved product {ProductId} with discount information", request.Id);
        return new GetProductWithDiscountByIdQueryResult(product);
    }
}
