using Catalog.API.DTOs;

namespace Catalog.API.Extensions;

/// <summary>
/// Helper class for calculating discount amounts based on different discount types.
/// </summary>
public static class DiscountCalculator
{
    /// <summary>
    /// Calculates the final price after applying the discount.
    /// </summary>
    /// <param name="originalPrice">The original price before discount.</param>
    /// <param name="discount">The discount information.</param>
    /// <returns>The final price after discount.</returns>
    public static decimal CalculateFinalPrice(decimal originalPrice, DiscountInfoDto? discount)
    {
        if (discount is null || !discount.HasDiscount)
            return originalPrice;

        return discount.DiscountType switch
        {
            1 => CalculatePercentageDiscount(originalPrice, discount.Percentage),      // Percentage
            2 => CalculateFixedAmountDiscount(originalPrice, discount.Amount),         // FixedAmount
            3 => CalculateCombinedDiscount(originalPrice, discount.Percentage, discount.Amount), // Combined
            _ => originalPrice
        };
    }

    /// <summary>
    /// Calculates the total discount amount applied.
    /// </summary>
    /// <param name="originalPrice">The original price before discount.</param>
    /// <param name="discount">The discount information.</param>
    /// <returns>The total amount discounted.</returns>
    public static decimal CalculateDiscountAmount(decimal originalPrice, DiscountInfoDto? discount)
    {
        if (discount is null || !discount.HasDiscount)
            return 0;

        var finalPrice = CalculateFinalPrice(originalPrice, discount);
        return originalPrice - finalPrice;
    }

    private static decimal CalculatePercentageDiscount(decimal price, double percentage)
    {
        return price * (1 - (decimal)percentage / 100);
    }

    private static decimal CalculateFixedAmountDiscount(decimal price, double amount)
    {
        var discountedPrice = price - (decimal)amount;
        return discountedPrice > 0 ? discountedPrice : 0; // Ensure price doesn't go negative
    }

    private static decimal CalculateCombinedDiscount(decimal price, double percentage, double amount)
    {
        // First apply percentage discount
        var priceAfterPercentage = CalculatePercentageDiscount(price, percentage);
        
        // Then apply fixed amount discount
        return CalculateFixedAmountDiscount(priceAfterPercentage, amount);
    }
}
