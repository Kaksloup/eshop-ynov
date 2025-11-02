namespace Discount.Grpc.Models;

/// <summary>
/// Defines the types of discounts that can be applied to products.
/// </summary>
public enum DiscountType
{
    /// <summary>
    /// No discount applied.
    /// </summary>
    None = 0,
    
    /// <summary>
    /// Percentage-based discount (e.g., 20% off).
    /// </summary>
    Percentage = 1,
    
    /// <summary>
    /// Fixed amount discount (e.g., 10€ off).
    /// </summary>
    FixedAmount = 2,
    
    /// <summary>
    /// Combined discount: percentage + fixed amount (e.g., 10% off + 5€ off).
    /// Both percentage and fixed amount are applied sequentially.
    /// </summary>
    Combined = 3
}
