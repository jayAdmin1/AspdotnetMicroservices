using FluentValidation;
using Registration.API.ViewModels;

namespace Registration.API.Validations
{
    public class LoginValidation : AbstractValidator<UserLoginModel>
    {
        public LoginValidation()
        {
            RuleFor(m => m.EmailAddress)
            .NotEmpty()
            .WithMessage("Email Address Required")
            .Matches(@"([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")
            .WithMessage("Enter Valid Email Address");

            RuleFor(m => m.Password)
                .NotEmpty().WithMessage("Password Required");
        }
    }
}
