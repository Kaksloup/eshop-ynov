using BuildingBlocks.CQRS;

namespace Catalog.API.Features.Products.Commands.DeleteProduct;

public class DeleteProductCommand: ICommand<DeleteProductCommandResult>
{
    public Guid Id { get; set; }
}