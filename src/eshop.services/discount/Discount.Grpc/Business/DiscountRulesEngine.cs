namespace Discount.Grpc.Business;

/// <summary>
/// Business rules engine for discount validation and stacking logic.
/// </summary>
public static class DiscountRulesEngine
{
    /// <summary>
    /// Maximum cumulative percentage discount allowed (30%).
    /// </summary>
    public const double MaxCumulativePercentage = 30.0;

    /// <summary>
    /// Maximum cumulative fixed amount discount allowed.
    /// </summary>
    public const double MaxCumulativeAmount = 100.0;

    /// <summary>
    /// Validates if a discount can be applied based on business rules.
    /// </summary>
    public static DiscountValidationResult ValidateDiscount(
        Models.Coupon coupon,
        List<Models.Coupon>? existingCoupons = null)
    {
        // Rule 1: Coupon must be active
        if (!coupon.IsActive)
        {
            return DiscountValidationResult.Failure("Coupon is not active");
        }

        // If no existing coupons, allow
        if (existingCoupons == null || existingCoupons.Count == 0)
        {
            return DiscountValidationResult.Success();
        }

        // Rule 2: Check if any existing coupon is non-stackable
        if (existingCoupons.Any(c => !c.IsStackable))
        {
            return DiscountValidationResult.Failure("Cannot stack with non-stackable coupon");
        }

        // Rule 3: Current coupon must be stackable
        if (!coupon.IsStackable)
        {
            return DiscountValidationResult.Failure("This coupon cannot be combined with others");
        }

        // Rule 4: Check cumulative percentage limit
        var totalPercentage = existingCoupons
            .Where(c => c.DiscountType == Models.DiscountType.Percentage || c.DiscountType == Models.DiscountType.Combined)
            .Sum(c => c.Percentage);

        totalPercentage += coupon.DiscountType == Models.DiscountType.Percentage || coupon.DiscountType == Models.DiscountType.Combined
            ? coupon.Percentage
            : 0;

        if (totalPercentage > MaxCumulativePercentage)
        {
            return DiscountValidationResult.Failure(
                $"Cumulative percentage discount ({totalPercentage}%) exceeds maximum allowed ({MaxCumulativePercentage}%)");
        }

        // Rule 5: Check cumulative amount limit
        var totalAmount = existingCoupons
            .Where(c => c.DiscountType == Models.DiscountType.FixedAmount || c.DiscountType == Models.DiscountType.Combined)
            .Sum(c => c.Amount);

        totalAmount += coupon.DiscountType == Models.DiscountType.FixedAmount || coupon.DiscountType == Models.DiscountType.Combined
            ? coupon.Amount
            : 0;

        if (totalAmount > MaxCumulativeAmount)
        {
            return DiscountValidationResult.Failure(
                $"Cumulative fixed discount ({totalAmount}€) exceeds maximum allowed ({MaxCumulativeAmount}€)");
        }

        // Rule 6: Prevent duplicate coupons on same product
        if (existingCoupons.Any(c => c.ProductName == coupon.ProductName && c.Id != coupon.Id))
        {
            return DiscountValidationResult.Failure(
                $"A discount is already applied to product '{coupon.ProductName}'");
        }

        return DiscountValidationResult.Success();
    }

    /// <summary>
    /// Calculates the total discount from multiple coupons.
    /// </summary>
    public static (double TotalPercentage, double TotalAmount) CalculateCumulativeDiscount(List<Models.Coupon> coupons)
    {
        var totalPercentage = coupons
            .Where(c => c.IsActive && (c.DiscountType == Models.DiscountType.Percentage || c.DiscountType == Models.DiscountType.Combined))
            .Sum(c => c.Percentage);

        var totalAmount = coupons
            .Where(c => c.IsActive && (c.DiscountType == Models.DiscountType.FixedAmount || c.DiscountType == Models.DiscountType.Combined))
            .Sum(c => c.Amount);

        return (
            Math.Min(totalPercentage, MaxCumulativePercentage),
            Math.Min(totalAmount, MaxCumulativeAmount)
        );
    }
}

/// <summary>
/// Result of discount validation.
/// </summary>
public class DiscountValidationResult
{
    public bool IsValid { get; set; }
    public string? ErrorMessage { get; set; }

    public static DiscountValidationResult Success() => new() { IsValid = true };
    public static DiscountValidationResult Failure(string message) => new() { IsValid = false, ErrorMessage = message };
}
