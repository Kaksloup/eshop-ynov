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
}
