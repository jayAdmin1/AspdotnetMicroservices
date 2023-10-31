using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Commands.UpdateOrder
{
    public class UpdateOrderCommandValidator:AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            RuleFor(p => p.UserName)
                .NotEmpty().WithMessage("{UserName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{UserName} must not exceed 50 characters.");

            RuleFor(p => p.EmailAddress)
                    .NotEmpty().WithMessage("{EmailAddress} is required.")
                     .Matches(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$").WithMessage("Enter Valid Email Address.");

            RuleFor(p => p.TotalPrice)
                    .NotEmpty().WithMessage("{TotalPrice} is required")
                    .GreaterThan(0).WithMessage("{TotalPrice} should be greater than zero.");
        }
        
    }
}
