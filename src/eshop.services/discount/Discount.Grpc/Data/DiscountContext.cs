using Discount.Grpc.Models;
using Marten;

namespace Discount.Grpc.Data;

/// <summary>
/// Marten configuration for the Discount service.
/// This replaces the EF Core DbContext with Marten's document store.
/// </summary>
public static class DiscountStoreConfiguration
{
    /// <summary>
    /// Configures Marten for storing Coupon documents in PostgreSQL.
    /// </summary>
    public static void ConfigureMarten(this StoreOptions options)
    {
        // Configure the Coupon document
        options.Schema.For<Coupon>()
            .Identity(x => x.Id)
            .DocumentAlias("coupons");
    }
}