using FluentValidation;

namespace Catalog.API.Features.Products.Commands.UpdateProduct;

/// <summary>
/// Validates the UpdateProductCommand to ensure that all required properties meet the defined rules and constraints.
/// </summary>
/// <remarks>
/// Utilizes FluentValidation to define validation rules for properties of the UpdateProductCommand.
/// This validator ensures that the data provided for updating a product is correct and adheres to business logic constraints.
/// </remarks>
public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    /// <summary>
    /// Provides validation rules for the CreateProductCommand.
    /// </summary>
    /// <remarks>
    /// Ensures that the command meets necessary requirements such as non-empty properties
    /// and valid data constraints for creating a product.
    /// </remarks>
    public UpdateProductCommandValidator()
    {
        RuleFor(product => product.Name).NotEmpty().WithMessage("Name is required").When(x => x.Name is not null);
        RuleFor(product => product.Categories).NotEmpty().WithMessage("Categories are required").When(x => x.Categories is not null);
        RuleFor(product => product.ImageFile).NotEmpty().WithMessage("ImageFile is required").When(x => x.ImageFile is not null);
        RuleFor(product => product.Description).NotEmpty().WithMessage("Description is required").When(x => x.Description is not null);
        RuleFor(product => product.Price).GreaterThanOrEqualTo(1).WithMessage("Price must be greater than or equal to 1").When(x => x.Price.HasValue);

        RuleFor(x => x).Must(x => x.Name != null || x.Price.HasValue || x.Description != null || x.ImageFile != null || x.Categories != null)
            .WithMessage("At least one field must be provided for update.");
    }
}