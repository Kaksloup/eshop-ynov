using Discount.Grpc.Models;

namespace Discount.Grpc.DTOs;

/// <summary>
/// DTO for creating a new coupon.
/// </summary>
public record CreateCouponDto
{
    public string ProductName { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public Models.DiscountType DiscountType { get; init; } = Models.DiscountType.Percentage;
    public double Percentage { get; init; }
    public double Amount { get; init; }
    public string? CouponCode { get; init; }
    public bool IsStackable { get; init; } = true;
    public double MaxStackablePercentage { get; init; } = 30.0;
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public double MinimumPurchaseAmount { get; init; } = 0.0;
}

/// <summary>
/// DTO for updating an existing coupon.
/// </summary>
public record UpdateCouponDto
{
    public string? ProductName { get; init; }
    public string? Description { get; init; }
    public Models.DiscountType? DiscountType { get; init; }
    public double? Percentage { get; init; }
    public double? Amount { get; init; }
    public string? CouponCode { get; init; }
    public bool? IsStackable { get; init; }
    public double? MaxStackablePercentage { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public double? MinimumPurchaseAmount { get; init; }
    public bool? IsActive { get; init; }
}

/// <summary>
/// DTO for coupon response.
/// </summary>
public record CouponDto
{
    public int Id { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public Models.DiscountType DiscountType { get; init; }
    public double Percentage { get; init; }
    public double Amount { get; init; }
    public string? CouponCode { get; init; }
    public bool IsActive { get; init; }
    public bool IsStackable { get; init; }
    public double MaxStackablePercentage { get; init; }
    public CouponStatus Status { get; init; }
    public CouponStatus EffectiveStatus { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public double MinimumPurchaseAmount { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}

/// <summary>
/// DTO for coupon list filters.
/// </summary>
public record CouponFilterDto
{
    public CouponStatus? Status { get; init; }
    public string? ProductName { get; init; }
    public string? CouponCode { get; init; }
    public bool? IsActive { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}
