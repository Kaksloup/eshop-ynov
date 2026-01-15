using BuildingBlocks.CQRS;
using Catalog.API.Exceptions;
using Catalog.API.Features.Products.Commands.CreateProduct;
using Catalog.API.Features.Products.Commands.UpdateProduct;
using Catalog.API.Models;
using FluentValidation;
using Mapster;
using Marten;
using MediatR;
using OfficeOpenXml;

namespace Catalog.API.Features.Products.Commands.ImportProduct;

/// <summary>
/// Handles the ImportProduct command to create a new product in the system by persisting it through the provided document session.
/// </summary>
public class ImportProductCommandHandler(IDocumentSession documentSession, ISender sender): ICommandHandler<ImportProductCommand, ImportProductCommandResult>
{
    /// <summary>
    /// Handles the processing of the CreateProduct command, which adds a new product to the system.
    /// It ensures the product does not already exist and persists it to the database.
    /// </summary>
    /// <param name="request">The CreateProduct command containing the details of the product to create.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
    /// <returns>A task representing the operation, containing the result of the command which includes the product ID.</returns>
    /// <exception cref="ProductAlreadyExistsException">Thrown when a product with the same name already exists in the system.</exception>
    public async Task<ImportProductCommandResult> Handle(ImportProductCommand request,
        CancellationToken cancellationToken)
    {
        
        var errors = new List<string>();
        var createErrors = new List<string>();
        int created = 0;
        int updated = 0;
        int total = 0;

        try
        {
            ExcelPackage.License.SetNonCommercialPersonal("products");

            using var stream = new MemoryStream();
            await request.FormFile.CopyToAsync(stream, cancellationToken);
            stream.Seek(0, SeekOrigin.Begin);

            using var package = new ExcelPackage(stream);
            var worksheet = package.Workbook.Worksheets[0];
            
            var excelValidator = new ExcelWorksheetValidator();
            var validationResult = excelValidator.Validate(worksheet);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);
            
            var rowCount = worksheet.Dimension?.Rows ?? 0;

            for (int row = 2; row <= rowCount; row++)
            {
                try
                {
                    var name = worksheet.Cells[row, 1].Value?.ToString();
                    var description = worksheet.Cells[row, 2].Value?.ToString();
                    var priceString = worksheet.Cells[row, 3].Value?.ToString();
                    var imageFile = worksheet.Cells[row, 4].Value?.ToString();
                    var categoryString = worksheet.Cells[row, 5].Value?.ToString();
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        errors.Add($"Row {row}: Name is required");
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(description))
                    {
                        errors.Add($"Row {row}: Description is required");
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(imageFile))
                    {
                        errors.Add($"Row {row}: ImageFile is required");
                        continue;
                    }

                    if (!decimal.TryParse(priceString, out var price) || price < 1)
                    {
                        errors.Add($"Row {row}: Price must be a valid number >= 1");
                        continue;
                    }

                    var categories = categoryString?.Split(',')
                        .Select(c => c.Trim())
                        .Where(c => !string.IsNullOrWhiteSpace(c))
                        .ToList() ?? new List<string>();

                    // Check if product exists
                    var existingProduct = await documentSession.Query<Product>()
                        .FirstOrDefaultAsync(
                            x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase),
                            cancellationToken);

                    if (existingProduct != null)
                    {
                        // Update existing product
                        var updateCommand = new UpdateProductCommand(
                            existingProduct.Id,
                            name,
                            description ?? string.Empty,
                            price,
                            imageFile ?? string.Empty,
                            categories
                        );

                        await sender.Send(updateCommand, cancellationToken);
                        updated++;
                    }
                    else
                    {
                        // Create new product
                        var createCommand = new CreateProductCommand
                        {
                            Name = name,
                            Description = description,
                            Price = price,
                            ImageFile =  imageFile,
                            Categories =  categories,
                        };

                        await sender.Send(createCommand, cancellationToken);
                        created++;
                    }

                    total++;
                }
                catch (Exception ex)
                {
                    errors.Add($"Row {row}: {ex.Message}");
                }
            }
            return new ImportProductCommandResult(
                isSuccessful: errors.Count == 0,
                total: total,
                created: created,
                updated: updated,
                errors
            );
        }
        catch (Exception ex)
        {
            errors.Add($"File processing error: {ex.Message}");
            return new ImportProductCommandResult(false, total, created, updated, errors);
        }
    }
}