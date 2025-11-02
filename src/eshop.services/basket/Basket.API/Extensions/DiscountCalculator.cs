namespace Basket.API.Extensions;

/// <summary>
/// Helper class for calculating discount amounts in the basket.
/// </summary>
public static class DiscountCalculator
{
    /// <summary>
    /// Applies a discount to a price based on the discount type.
    /// </summary>
    /// <param name="originalPrice">The original price before discount.</param>
    /// <param name="discountType">The type of discount (1=Percentage, 2=FixedAmount, 3=Combined).</param>
    /// <param name="percentage">The percentage discount (0-100).</param>
    /// <param name="amount">The fixed amount discount.</param>
    /// <returns>The final price after applying the discount.</returns>
    public static decimal ApplyDiscount(decimal originalPrice, int discountType, double percentage, double amount)
    {
        return discountType switch
        {
            1 => ApplyPercentageDiscount(originalPrice, percentage),         // Percentage
            2 => ApplyFixedAmountDiscount(originalPrice, amount),            // FixedAmount
            3 => ApplyCombinedDiscount(originalPrice, percentage, amount),   // Combined
            _ => originalPrice
        };
    }

    private static decimal ApplyPercentageDiscount(decimal price, double percentage)
    {
        if (percentage <= 0) return price;
        return price * (1 - (decimal)percentage / 100);
    }

    private static decimal ApplyFixedAmountDiscount(decimal price, double amount)
    {
        if (amount <= 0) return price;
        var discountedPrice = price - (decimal)amount;
        return discountedPrice > 0 ? discountedPrice : 0;
    }

    private static decimal ApplyCombinedDiscount(decimal price, double percentage, double amount)
    {
        // First apply percentage
        var priceAfterPercentage = ApplyPercentageDiscount(price, percentage);
        
        // Then apply fixed amount
        return ApplyFixedAmountDiscount(priceAfterPercentage, amount);
    }
}
