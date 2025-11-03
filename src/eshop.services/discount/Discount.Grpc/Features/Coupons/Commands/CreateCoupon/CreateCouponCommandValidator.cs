using FluentValidation;

namespace Discount.Grpc.Features.Coupons.Commands.CreateCoupon;

/// <summary>
/// Validator for CreateCouponCommand.
/// </summary>
public class CreateCouponCommandValidator : AbstractValidator<CreateCouponCommand>
{
    public CreateCouponCommandValidator()
    {
        RuleFor(x => x.Coupon.ProductName)
            .NotEmpty().WithMessage("Product name is required")
            .MaximumLength(200).WithMessage("Product name must not exceed 200 characters");

        RuleFor(x => x.Coupon.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters");

        RuleFor(x => x.Coupon.Percentage)
            .InclusiveBetween(0, 100).WithMessage("Percentage must be between 0 and 100");

        RuleFor(x => x.Coupon.Amount)
            .GreaterThanOrEqualTo(0).WithMessage("Amount must be greater than or equal to 0");

        RuleFor(x => x.Coupon.MaxStackablePercentage)
            .InclusiveBetween(0, 100).WithMessage("Max stackable percentage must be between 0 and 100");

        RuleFor(x => x.Coupon.MinimumPurchaseAmount)
            .GreaterThanOrEqualTo(0).WithMessage("Minimum purchase amount must be greater than or equal to 0");

        RuleFor(x => x.Coupon)
            .Must(x => x.EndDate == null || x.StartDate == null || x.EndDate > x.StartDate)
            .WithMessage("End date must be after start date");

        RuleFor(x => x.Coupon.CouponCode)
            .MaximumLength(50).WithMessage("Coupon code must not exceed 50 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Coupon.CouponCode));
    }
}
