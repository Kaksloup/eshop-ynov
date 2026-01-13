namespace Catalog.API.Features.Products.Commands.ImportProduct;

/// <summary>
/// Represents the result of the ImportProduct command execution.
/// </summary>
public record ImportProductCommandResult(bool isSuccessful, int total, int created, int updated, List<string> errors);