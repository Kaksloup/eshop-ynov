using Catalog.API.Features.Products.Commands.CreateProduct;
using Catalog.API.Models;
using FluentValidation;
using OfficeOpenXml;

namespace Catalog.API.Features.Products.Commands.ImportProduct;

/// <summary>
/// Validates the ImportProductCommand to ensure that all required properties meet the defined rules and constraints.
/// </summary>
/// <remarks>
/// Utilizes FluentValidation to define validation rules for properties of the ImportProductCommand.
/// This validator ensures that the data provided for importing a correct file and adheres to business logic constraints.
/// </remarks>
public class ImportProductCommandValidator: AbstractValidator<ImportProductCommand>
{
    /// <summary>
    /// Provides validation rules for the CreateProductCommand.
    /// </summary>
    /// <remarks>
    /// Ensures that the command meets necessary requirements such as non-empty properties
    /// and valid data constraints for creating a product.
    /// </remarks>
    private static readonly string[] AllowedExtensions = { ".xlsx", ".xls", ".csv" };
    
    public ImportProductCommandValidator()
    {
        RuleFor(x => x.FormFile)
            .NotNull().WithMessage("File is required")
            .Must(file => file.Length > 0).WithMessage("File is empty")
            .Must(file => AllowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            .WithMessage($"Only {string.Join(", ", AllowedExtensions)} files are allowed");
    }
}