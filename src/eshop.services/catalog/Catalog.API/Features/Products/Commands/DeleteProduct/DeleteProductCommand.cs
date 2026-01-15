using BuildingBlocks.CQRS;

namespace Catalog.API.Features.Products.Commands.DeleteProduct;
/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
public record DeleteProductCommand(
    Guid Id
    ): ICommand<DeleteProductCommandResult>;
