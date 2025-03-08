using ECommerce.Core.Entities;
using FluentValidation;

namespace ECommerce.Core.Validators;

public class ProductRequestValidator : AbstractValidator<Product>
{
    public ProductRequestValidator()
    {
        
    }
}