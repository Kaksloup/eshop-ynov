using Catalog.API.DTOs;

namespace Catalog.API.Models;

/// <summary>
/// Represents a product within the catalog. Provides details such as product name, description,
/// price, associated categories, and an image file.
/// </summary>
public class Product
{
    /// <summary>
    /// Gets or sets the unique identifier for the product.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the product.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the product.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the base price of the product (without discount).
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the name of the image file associated with the product.
    /// </summary>
    public string ImageFile { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of categories associated with the product.
    /// </summary>
    public List<string> Categories { get; set; } = [];

    /// <summary>
    /// Gets or sets the discount information for this product (if any).
    /// This property is populated dynamically from the Discount Service and not stored in the database.
    /// </summary>
    public DiscountInfoDto? Discount { get; set; }

    /// <summary>
    /// Gets the final price after applying the discount (if any).
    /// Supports multiple discount types: Percentage, FixedAmount, and Combined.
    /// If no discount exists or is inactive, returns the base Price.
    /// </summary>
    public decimal FinalPrice => Catalog.API.Extensions.DiscountCalculator.CalculateFinalPrice(Price, Discount);
    
    /// <summary>
    /// Gets the total amount saved with the discount.
    /// </summary>
    public decimal DiscountAmount => Catalog.API.Extensions.DiscountCalculator.CalculateDiscountAmount(Price, Discount);
}