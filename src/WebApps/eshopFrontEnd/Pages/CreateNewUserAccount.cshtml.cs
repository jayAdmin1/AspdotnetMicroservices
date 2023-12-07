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
        public async Task<IActionResult> OnGet()
        { 
            user = new UserAddModel();
            return Page();
        }
        public async Task<IActionResult> OnPostAddUserAsync(UserAddModel userAddModel)
        {
            var result = await _userService.CreateUser(userAddModel);
            return !string.IsNullOrEmpty(result.Item2.Message) ? RedirectToPage("error", result.Item2) : RedirectToPage("Login");
        }
    }
}
