namespace Discount.Grpc.Models;

/// <summary>
/// Represents a discount coupon with multiple reduction types.
/// </summary>
public class Coupon
{
    /// <summary>
    /// Gets or sets the unique identifier of the coupon.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Gets or sets the product name associated with this coupon.
    /// </summary>
    public string ProductName { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the description of the discount (e.g., "Black Friday", "Summer Sale").
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the type of discount (Percentage, FixedAmount, or Combined).
    /// </summary>
    public DiscountType DiscountType { get; set; } = DiscountType.Percentage;
    
    /// <summary>
    /// Gets or sets the discount percentage (0-100).
    /// Used when DiscountType is Percentage or Combined.
    /// Example: 20.0 means 20% off.
    /// </summary>
    public double Percentage { get; set; }
    
    /// <summary>
    /// Gets or sets the fixed discount amount.
    /// Used when DiscountType is FixedAmount or Combined.
    /// Example: 5.0 means 5€ off.
    /// </summary>
    public double Amount { get; set; }
    
    /// <summary>
    /// Gets or sets the optional coupon code (e.g., "SUMMER2024", "BLACKFRIDAY").
    /// </summary>
    public string? CouponCode { get; set; }
    
    /// <summary>
    /// Gets or sets whether this coupon is currently active.
    /// </summary>
    public bool IsActive { get; set; } = true;
    
    /// <summary>
    /// Gets or sets whether this coupon can be stacked/combined with other coupons.
    /// </summary>
    public bool IsStackable { get; set; } = true;
    
    /// <summary>
    /// Gets or sets the maximum total percentage when stacking multiple coupons (0-100).
    /// Example: 30.0 means total discount cannot exceed 30%.
    /// Only applies when IsStackable is true.
    /// </summary>
    public double MaxStackablePercentage { get; set; } = 30.0;
    
    /// <summary>
    /// Gets or sets the current status of the coupon.
    /// </summary>
    public CouponStatus Status { get; set; } = CouponStatus.Active;
    
    /// <summary>
    /// Gets or sets the start date when this coupon becomes active.
    /// Null means the coupon is active immediately.
    /// </summary>
    public DateTime? StartDate { get; set; }
    
    /// <summary>
    /// Gets or sets the end date when this coupon expires.
    /// Null means the coupon never expires.
    /// </summary>
    public DateTime? EndDate { get; set; }
    
    /// <summary>
    /// Gets or sets the minimum purchase amount required to use this coupon.
    /// </summary>
    public double MinimumPurchaseAmount { get; set; } = 0.0;
    
    /// <summary>
    /// Gets or sets when this coupon was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Gets or sets when this coupon was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
    
    /// <summary>
    /// Determines the current effective status of the coupon based on dates and IsActive flag.
    /// </summary>
    public CouponStatus GetEffectiveStatus()
    {
        if (!IsActive)
            return CouponStatus.Disabled;
            
        var now = DateTime.UtcNow;
        
        if (StartDate.HasValue && now < StartDate.Value)
            return CouponStatus.Upcoming;
            
        if (EndDate.HasValue && now > EndDate.Value)
            return CouponStatus.Expired;
            
        return CouponStatus.Active;
    }
}