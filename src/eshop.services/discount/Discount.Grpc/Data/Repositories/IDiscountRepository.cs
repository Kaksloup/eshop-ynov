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

    /// <summary>
    /// Retrieves a paginated and filtered list of coupons.
    /// </summary>
    /// <param name="status">Optional status filter.</param>
    /// <param name="productName">Optional product name filter.</param>
    /// <param name="couponCode">Optional coupon code filter.</param>
    /// <param name="isActive">Optional active status filter.</param>
    /// <param name="pageNumber">Page number (1-based).</param>
    /// <param name="pageSize">Number of items per page.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Tuple containing the list of coupons and total count.</returns>
    Task<(List<Coupon> Items, int TotalCount)> GetPagedAsync(
        CouponStatus? status = null,
        string? productName = null,
        string? couponCode = null,
        bool? isActive = null,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a coupon by its coupon code.
    /// </summary>
    /// <param name="couponCode">The coupon code to search for.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The coupon if found, otherwise null.</returns>
    Task<Coupon?> GetByCouponCodeAsync(string couponCode, CancellationToken cancellationToken = default);
}
