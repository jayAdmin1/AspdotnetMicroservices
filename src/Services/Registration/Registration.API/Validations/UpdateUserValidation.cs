using FluentValidation;
using Registration.API.ViewModels;

namespace Registration.API.Validations
{
    public class UpdateUserValidation : AbstractValidator<UserUpdateModel>
    {
        public UpdateUserValidation()
        {
            RuleFor(m => m.Name)
               .NotEmpty().WithMessage("Name Requried")
               .Matches("^[a-zA-Z ]+$").WithMessage("Name should have alphabets only.")
               .Length(2, 20).WithMessage("Name length is between 2 to 20 characters.");

            RuleFor(m => m.MobileNo)
                .NotEmpty().WithMessage("Mobile No. Required")
                .Matches(@"(^[0-9]{10}$)|(^\+[0-9]{1,3}[0-9]{10}$)|(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)").WithMessage("Enter Valid Mobile No.");

            RuleFor(m => m.Address)
                .NotEmpty().WithMessage("Address Required");
        }
    }
}
