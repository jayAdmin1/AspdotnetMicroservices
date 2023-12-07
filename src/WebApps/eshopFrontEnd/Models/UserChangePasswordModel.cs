using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace eshopFrontEnd.Models
{
    public class UserChangePasswordModel
    {
        [BindProperty(Name = "UserChangePasswordModel.OldPassword")]
        [Required(ErrorMessage = "Old Password Required")]
        public string OldPassword { get; set; }

        [BindProperty(Name = "UserChangePasswordModel.NewPassword")]
        [Required(ErrorMessage = "New Password Required")]
        public string NewPassword { get; set; }
    }
}
