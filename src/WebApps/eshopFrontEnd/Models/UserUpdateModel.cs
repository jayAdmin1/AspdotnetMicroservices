using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace eshopFrontEnd.Models
{
    public class UserUpdateModel
    {
        //public Guid Id { get; set; }

        [BindProperty(Name = "userUpdateModel.Name")]
        [Required(ErrorMessage = "Name Required")]
        [RegularExpression(@"^[a-zA-Z ]+$",ErrorMessage = "Invalid Name")]
        public string Name { get; set; }


        [BindProperty(Name = "userUpdateModel.MobileNo")]
        [Required(ErrorMessage = "Mobile No. Required")]
        [RegularExpression(@"(^[0-9]{10}$)|(^\+[0-9]{1,3}[0-9]{10}$)|(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)", ErrorMessage = "Invalid Mobile No.")]
        public string MobileNo { get; set; }

        [BindProperty(Name = "userUpdateModel.Address")]
        [Required(ErrorMessage = "Address Required")]
        public string Address { get; set; }
    }
}
