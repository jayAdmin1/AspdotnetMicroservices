using FluentValidation;
using Registration.API.ViewModels;

namespace Registration.API.Validations
{
    public class UserPasswordValidation : AbstractValidator<UserPasswordModel>
    {
        public UserPasswordValidation()
        {
            RuleFor(m => m.OldPassword)
                .NotEmpty().WithMessage("OldPassword Required");
            RuleFor(m => m.NewPassword)
                .NotEmpty().WithMessage("NewPassword Required");
        }
    }
}
