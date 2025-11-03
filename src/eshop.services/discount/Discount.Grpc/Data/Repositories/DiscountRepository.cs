using Discount.Grpc.Models;
using Marten;

namespace Discount.Grpc.Data.Repositories;

/// <summary>
/// Repository implementation for managing discount coupons using Marten (PostgreSQL Document Store).
/// Architecture aligned with Catalog service.
/// </summary>
public class DiscountRepository(IDocumentSession documentSession) : IDiscountRepository
{
    /// <inheritdoc />
    public async Task<Coupon?> GetByProductNameAsync(string productName, CancellationToken cancellationToken = default)
    {
        return await documentSession.Query<Coupon>()
            .FirstOrDefaultAsync(x => x.ProductName == productName, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Coupon?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await documentSession.LoadAsync<Coupon>(id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Coupon> CreateAsync(Coupon coupon, CancellationToken cancellationToken = default)
    {
        documentSession.Store(coupon);
        await documentSession.SaveChangesAsync(cancellationToken);
        return coupon;
    }

    /// <inheritdoc />
    public async Task<Coupon> UpdateAsync(Coupon coupon, CancellationToken cancellationToken = default)
    {
        documentSession.Update(coupon);
        await documentSession.SaveChangesAsync(cancellationToken);
        return coupon;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(Coupon coupon, CancellationToken cancellationToken = default)
    {
        documentSession.Delete(coupon);
        await documentSession.SaveChangesAsync(cancellationToken);
        return true;
    }

    /// <inheritdoc />
    public async Task<List<Coupon>> GetActiveByProductNameAsync(string productName, CancellationToken cancellationToken = default)
    {
        var result = await documentSession.Query<Coupon>()
            .Where(x => x.ProductName == productName && x.IsActive)
            .ToListAsync(cancellationToken);
        return result.ToList();
    }

    /// <inheritdoc />
    public async Task<(List<Coupon> Items, int TotalCount)> GetPagedAsync(
        CouponStatus? status = null,
        string? productName = null,
        string? couponCode = null,
        bool? isActive = null,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        // Get all coupons and filter in memory
        // For production use, consider implementing server-side filtering with Marten's compiled queries
        var allCoupons = await documentSession.Query<Coupon>()
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);

        // Apply filters in memory
        var filtered = allCoupons.AsEnumerable();

        if (status.HasValue)
        {
            filtered = filtered.Where(x => x.Status == status.Value);
        }

        if (!string.IsNullOrWhiteSpace(productName))
        {
            filtered = filtered.Where(x => x.ProductName.Contains(productName, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(couponCode))
        {
            filtered = filtered.Where(x => x.CouponCode != null && x.CouponCode.Contains(couponCode, StringComparison.OrdinalIgnoreCase));
        }

        if (isActive.HasValue)
        {
            filtered = filtered.Where(x => x.IsActive == isActive.Value);
        }

        var filteredList = filtered.ToList();
        var totalCount = filteredList.Count;

        // Apply pagination
        var items = filteredList
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return (items, totalCount);
    }

    /// <inheritdoc />
    public async Task<Coupon?> GetByCouponCodeAsync(string couponCode, CancellationToken cancellationToken = default)
    {
        return await documentSession.Query<Coupon>()
            .FirstOrDefaultAsync(x => x.CouponCode == couponCode, cancellationToken);
    }
}
