using Discount.Grpc.Models;

namespace Discount.Grpc.Data.Repositories;

/// <summary>
/// Repository interface for managing discount coupons in the data store.
/// Provides abstraction over the data access layer for CRUD operations.
/// </summary>
public interface IDiscountRepository
{
    /// <summary>
    /// Retrieves a discount coupon by product name.
    /// </summary>
    /// <param name="productName">The name of the product to search for.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The coupon if found, otherwise null.</returns>
    Task<Coupon?> GetByProductNameAsync(string productName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a discount coupon by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the coupon.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The coupon if found, otherwise null.</returns>
    Task<Coupon?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new discount coupon in the data store.
    /// </summary>
    /// <param name="coupon">The coupon to create.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created coupon with generated ID.</returns>
    Task<Coupon> CreateAsync(Coupon coupon, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing discount coupon.
    /// </summary>
    /// <param name="coupon">The coupon with updated values.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated coupon.</returns>
    Task<Coupon> UpdateAsync(Coupon coupon, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a discount coupon from the data store.
    /// </summary>
    /// <param name="coupon">The coupon to delete.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if deletion was successful.</returns>
    Task<bool> DeleteAsync(Coupon coupon, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all active coupons for a specific product.
    /// </summary>
    /// <param name="productName">The name of the product.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of active coupons for the product.</returns>
    Task<List<Coupon>> GetActiveByProductNameAsync(string productName, CancellationToken cancellationToken = default);
}
