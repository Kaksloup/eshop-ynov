using Marten.Schema;

namespace Basket.API.Models;

/// <summary>
/// Represents a shopping cart for a specific user, containing a collection of items and providing functionality to calculate the total price.
/// </summary>
public class ShoppingCart
{
    [Identity]
    public string UserName { get; set; } = string.Empty;
    public IEnumerable<ShoppingCartItem> Items { get; set; } = [];
    
    /// <summary>
    /// Total price before any discounts are applied.
    /// </summary>
    public decimal BaseTotal => Items.Sum(item => item.BasePrice * item.Quantity);
    
    /// <summary>
    /// Total price after applying all discounts.
    /// </summary>
    public decimal Total => Items.Sum(item => item.Price * item.Quantity);
    
    /// <summary>
    /// Total amount saved through discounts.
    /// </summary>
    public decimal TotalSavings => BaseTotal - Total;

    public ShoppingCart(string userName)
    {
        UserName  = userName;
    }

    public ShoppingCart()
    {
        
    }
    
}