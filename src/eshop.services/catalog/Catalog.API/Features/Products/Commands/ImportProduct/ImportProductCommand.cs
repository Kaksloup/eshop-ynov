using BuildingBlocks.CQRS;
using Catalog.API.Features.Products.Commands.UpdateProduct;
using MediatR;

namespace Catalog.API.Features.Products.Commands.ImportProduct;

/// <summary>
/// Represents the command to import xlsx file.
/// </summary>
public record ImportProductCommand(IFormFile FormFile) : ICommand<ImportProductCommandResult>;