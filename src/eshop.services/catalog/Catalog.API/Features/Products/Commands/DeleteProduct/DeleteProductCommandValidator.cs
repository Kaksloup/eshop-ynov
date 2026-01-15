using FluentValidation;


namespace Catalog.API.Features.Products.Commands.DeleteProduct;
/// <summary>
///   
/// </summary>
public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    /// <summary>
    /// 
    /// </summary>
    public DeleteProductCommandValidator()
    {
        RuleFor(product => product.Id).NotEmpty().WithMessage("Id is required");

    }
}