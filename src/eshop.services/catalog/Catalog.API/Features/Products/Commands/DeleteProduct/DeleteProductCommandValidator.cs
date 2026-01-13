using FluentValidation;


namespace Catalog.API.Features.Products.Commands.DeleteProduct;

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(product => product.Id).NotEmpty().WithMessage("Id is required");

    }
}