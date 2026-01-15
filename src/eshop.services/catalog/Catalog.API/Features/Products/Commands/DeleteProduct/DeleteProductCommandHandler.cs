using BuildingBlocks.CQRS;
using Catalog.API.Exceptions;
using Catalog.API.Models;
using Marten;


namespace Catalog.API.Features.Products.Commands.DeleteProduct;

/// <summary>
/// 
/// </summary>
/// <param name="documentSession"></param>
public class DeleteProductCommandHandler(IDocumentSession documentSession) : ICommandHandler<DeleteProductCommand, DeleteProductCommandResult>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ProductNotFoundException"></exception>
    public async Task<DeleteProductCommandResult> Handle(DeleteProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = await documentSession.LoadAsync<Product>(request.Id, cancellationToken);

        if(product == null)
            throw new ProductNotFoundException(request.Id);

        documentSession.Delete(product);

        await documentSession.SaveChangesAsync(cancellationToken);

        return new DeleteProductCommandResult(IsSuccessful: true);
    }
    
}