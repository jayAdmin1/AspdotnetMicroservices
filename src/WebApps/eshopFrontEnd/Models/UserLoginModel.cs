using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace eshopFrontEnd.Models
{
    public class UserLoginModel
    {
        [BindProperty(Name = "userLogin.EmailAddress")]
        [Required(ErrorMessage = "Email Address Required")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
        ErrorMessage = "Invalid email format")]
        public string EmailAddress { get; set; }

        [BindProperty(Name = "userLogin.Password")]
        [Required(ErrorMessage = "Password Required")]
        public string Password { get; set; }
    }
}
