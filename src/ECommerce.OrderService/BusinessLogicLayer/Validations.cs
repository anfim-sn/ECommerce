using BusinessLogicLayer.DTO;
using FluentValidation;

namespace BusinessLogicLayer;

public class OrderAddRequestValidator : AbstractValidator<OrderAddRequest>
{
    public OrderAddRequestValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required");
        RuleFor(x => x.OrderDate).NotEmpty().WithMessage("OrderDate is required");
        RuleFor(x => x.OrderItems).NotEmpty().WithMessage("OrderItems is required");
    }
}

public class OrderItemAddRequestValidator : AbstractValidator<OrderItemAddRequest>
{
    public OrderItemAddRequestValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("ProductId is required");
        
        RuleFor(x => x.Quantity)
            .NotEmpty().WithMessage("Quantity is required")
            .GreaterThan(0).WithMessage("Quantity should be greater than 0");
        
        RuleFor(x => x.UnitPrice)
            .NotEmpty().WithMessage("Price is required")
            .GreaterThan(0).WithMessage("Quantity should be greater than 0");
    }
}

public class OrderUpdateRequestValidator : AbstractValidator<OrderUpdateRequest>
{
    public OrderUpdateRequestValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required");
        RuleFor(x => x.OrderId).NotEmpty().WithMessage("OrderId is required");
        RuleFor(x => x.OrderDate).NotEmpty().WithMessage("OrderDate is required");
        RuleFor(x => x.OrderItems).NotEmpty().WithMessage("OrderItems is required");
        
    }
}

public class OrderItemUpdateRequestValidator : AbstractValidator<OrderItemUpdateRequest>
{
    public OrderItemUpdateRequestValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("ProductId is required");

        RuleFor(x => x.Quantity)
            .NotEmpty().WithMessage("Quantity is required")
            .GreaterThan(0).WithMessage("Quantity should be greater than 0");

        RuleFor(x => x.UnitPrice)
            .NotEmpty().WithMessage("Price is required")
            .GreaterThan(0).WithMessage("Quantity should be greater than 0");
    }
}