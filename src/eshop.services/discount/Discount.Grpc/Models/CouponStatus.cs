namespace Discount.Grpc.Models;

/// <summary>
/// Defines the possible statuses for a coupon.
/// </summary>
public enum CouponStatus
{
    /// <summary>
    /// The coupon is currently active and can be used.
    /// </summary>
    Active = 0,
    
    /// <summary>
    /// The coupon has expired (end date has passed).
    /// </summary>
    Expired = 1,
    
    /// <summary>
    /// The coupon has been manually deactivated.
    /// </summary>
    Disabled = 2,
    
    /// <summary>
    /// The coupon will be active in the future (start date not yet reached).
    /// </summary>
    Upcoming = 3
}
