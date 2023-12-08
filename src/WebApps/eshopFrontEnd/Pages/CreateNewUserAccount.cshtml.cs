using eshopFrontEnd.Models;
using eshopFrontEnd.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eshopFrontEnd.Pages
{
    public class CreateNewUserAccountModel : PageModel
    {
        private readonly IUserService _userService;

        public CreateNewUserAccountModel(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public UserAddModel user { get; set; }
        public void OnGet()
        {
            user = new UserAddModel();
        }
        public async Task<IActionResult> OnPostAddUser(UserAddModel userAddModel)
        {
            if (TryValidateModel(userAddModel, nameof(user)))
            {
                var result = await _userService.CreateUser(userAddModel);
                if (!string.IsNullOrEmpty(result.Item2.Message))
                {
                    RedirectToPage("error", result.Item2);
                }
                RedirectToPage("Login");
            }
            return Page();
        }
    }
}
