namespace Catalog.API.Features.Products.Commands.UpdateProduct;

/// <summary>
/// Represents the result of executing the <see cref="UpdateProductCommand"/>.
/// </summary>
/// <remarks>
/// This result type indicates whether the product update operation was successful or not.
/// </remarks>
/// <param name="IsSuccessful">
/// A boolean value indicating if the update operation completed successfully.
/// </param>
public record UpdateProductCommandResult(bool IsSuccessful);