using BuildingBlocks.CQRS;
using Catalog.API.Exceptions;
using Catalog.API.Models;
using JasperFx.CodeGeneration.Frames;
using Mapster;
using Marten;
using Marten.Patching;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Features.Products.Commands.UpdateProduct;

/// <summary>
/// Handles the UpdateProduct command to update a new product in the system by persisting it through the provided document session.
/// </summary>
public class UpdateProductCommandHandler(IDocumentSession documentSession) : ICommandHandler<UpdateProductCommand, UpdateProductCommandResult>
{
    /// <summary>
    /// Handles the processing of the UpdateProduct command, which update an existing product to the system.
    /// It ensures the product does not already exist and persists it to the database.
    /// </summary>
    /// <param name="request">The UpdateProduct command containing the details of the product to update.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
    /// <returns>A boolean if the operation succesfully terminated.</returns>
    /// <exception cref="ProductNotFoundException">Thrown when a product was not found in the system.</exception>
    public async Task<UpdateProductCommandResult> Handle(UpdateProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = request.Adapt<UpdateProductCommand>();

        var check = await documentSession.Query<Product>()
            .AnyAsync(x => x.Id == product.Id, cancellationToken);

        if (!check)
            throw new ProductNotFoundException(product.Id);

        var patch = documentSession.Patch<Product>(product.Id);

        if (product.Name != null)
            patch.Set(x => x.Name, product.Name);

        if (product.Price.HasValue)
            patch.Set(x => x.Price, product.Price);

        if (product.Description != null)
            patch.Set(x => x.Description, product.Description);

        if (product.ImageFile != null)
            patch.Set(x => x.ImageFile, product.ImageFile);

        if (product.Categories != null)
            patch.Set(x => x.Categories, product.Categories);

        await documentSession.SaveChangesAsync(cancellationToken);

        return new UpdateProductCommandResult(true);
    }
}