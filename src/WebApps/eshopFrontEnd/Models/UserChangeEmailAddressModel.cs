using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace eshopFrontEnd.Models
{
    public class UserChangeEmailAddressModel
    {
        [BindProperty(Name = "UserChangeEmailAddressModel.OldEmailAddress")]
        [Required(ErrorMessage = "Old Email Address Required")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
        ErrorMessage = "Invalid email format")]
        public string OldEmailAddress { get; set; }

        [BindProperty(Name = "UserChangeEmailAddressModel.NewEmailAddress")]
        [Required(ErrorMessage = "New Email Address Required")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
        ErrorMessage = "Invalid email format")]
        public string NewEmailAddress { get; set; }
    }
}
