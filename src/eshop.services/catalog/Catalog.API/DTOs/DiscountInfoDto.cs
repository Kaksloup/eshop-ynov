namespace Catalog.API.DTOs;

/// <summary>
/// Data Transfer Object representing discount information for a product.
/// Maps to the Discount.Grpc.CouponModel from the Discount Service.
/// </summary>
public record DiscountInfoDto
{
    /// <summary>
    /// Gets or sets the identifier of the coupon/discount.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the product name associated with the discount.
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the discount (e.g., "Summer sale", "Black Friday").
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of discount (0=None, 1=Percentage, 2=FixedAmount, 3=Combined).
    /// </summary>
    public int DiscountType { get; set; }

    /// <summary>
    /// Gets or sets the discount percentage (0-100).
    /// Example: 20.0 means 20% discount.
    /// </summary>
    public double Percentage { get; set; }

    /// <summary>
    /// Gets or sets the fixed discount amount.
    /// Example: 5.0 means 5€ off.
    /// </summary>
    public double Amount { get; set; }

    /// <summary>
    /// Gets or sets the coupon code (e.g., "SUMMER2024").
    /// </summary>
    public string? CouponCode { get; set; }

    /// <summary>
    /// Gets or sets whether this discount is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets whether this discount can be stacked with other discounts.
    /// </summary>
    public bool IsStackable { get; set; }

    /// <summary>
    /// Indicates whether the product has an active discount.
    /// </summary>
    public bool HasDiscount => IsActive && (DiscountType > 0);
}
