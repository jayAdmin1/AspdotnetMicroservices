using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace eshopFrontEnd.Models
{
    public class UserAddModel
    {
        public Guid Id { get; set; }
        
        [BindProperty(Name = "user.Name")]
        [Required(ErrorMessage = "Name Required")]
        [RegularExpression(@"^[a-zA-Z ]+$",
        ErrorMessage = "Invalid Name")]
        public string Name { get; set; }

        [BindProperty(Name = "user.EmailAddress")]
        [Required(ErrorMessage = "Email Address Required")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
        ErrorMessage = "Invalid email format")]
        public string EmailAddress { get; set; }

        [BindProperty(Name = "user.MobileNo")]
        [Required(ErrorMessage = "Mobile No. Required")]
        [RegularExpression(@"(^[0-9]{10}$)|(^\+[0-9]{1,3}[0-9]{10}$)|(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)",ErrorMessage ="Invalid Mobile No.")]
        public string MobileNo { get; set; }

        [BindProperty(Name = "user.Address")]
        [Required(ErrorMessage = "Address Required")]
        public string Address { get; set; }

        [BindProperty(Name = "user.Password")]
        [Required(ErrorMessage = "Password Required")]
        public string Password { get; set; }
    }
}
