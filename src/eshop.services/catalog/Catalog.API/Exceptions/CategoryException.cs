using BuildingBlocks.Exceptions;

namespace Catalog.API.Exceptions;

/// <summary>
/// Represents an exception that is thrown when a specified category is not found
/// in the catalog. This exception is used to indicate that an operation could not
/// be completed due to the absence of the specified category.
/// </summary>
public class CategoryNotFoundException : NotFoundException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CategoryNotFoundException"/> class
    /// with the specified category name.
    /// </summary>
    public CategoryNotFoundException(string category)
        : base("Category", category) { }
}