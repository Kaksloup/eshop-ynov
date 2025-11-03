using FluentValidation;

namespace Discount.Grpc.Features.Coupons.Commands.UpdateCoupon;

/// <summary>
/// Validator for UpdateCouponCommand.
/// </summary>
public class UpdateCouponCommandValidator : AbstractValidator<UpdateCouponCommand>
{
    public UpdateCouponCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Coupon ID must be greater than 0");

        RuleFor(x => x.Coupon.ProductName)
            .MaximumLength(200).WithMessage("Product name must not exceed 200 characters")
            .When(x => x.Coupon.ProductName != null);

        RuleFor(x => x.Coupon.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters")
            .When(x => x.Coupon.Description != null);

        RuleFor(x => x.Coupon.Percentage)
            .InclusiveBetween(0, 100).WithMessage("Percentage must be between 0 and 100")
            .When(x => x.Coupon.Percentage.HasValue);

        RuleFor(x => x.Coupon.Amount)
            .GreaterThanOrEqualTo(0).WithMessage("Amount must be greater than or equal to 0")
            .When(x => x.Coupon.Amount.HasValue);

        RuleFor(x => x.Coupon.MaxStackablePercentage)
            .InclusiveBetween(0, 100).WithMessage("Max stackable percentage must be between 0 and 100")
            .When(x => x.Coupon.MaxStackablePercentage.HasValue);

        RuleFor(x => x.Coupon.MinimumPurchaseAmount)
            .GreaterThanOrEqualTo(0).WithMessage("Minimum purchase amount must be greater than or equal to 0")
            .When(x => x.Coupon.MinimumPurchaseAmount.HasValue);

        RuleFor(x => x.Coupon.CouponCode)
            .MaximumLength(50).WithMessage("Coupon code must not exceed 50 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Coupon.CouponCode));
    }
}
