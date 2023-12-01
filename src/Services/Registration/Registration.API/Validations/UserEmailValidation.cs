using FluentValidation;
using Registration.API.ViewModels;

namespace Registration.API.Validations
{
    public class UserEmailValidation : AbstractValidator<UserEmailModel>
    {
        public UserEmailValidation()
        {
            RuleFor(m => m.OldEmail)
            .NotEmpty()
            .WithMessage("Old Email Address Required")
            .Matches(@"([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")
            .WithMessage("Enter Valid Email Address");

            RuleFor(m => m.NewEmail)
            .NotEmpty()
            .WithMessage("New Email Address Required")
            .Matches(@"([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")
            .WithMessage("Enter Valid Email Address");

            
        }
    }
}
