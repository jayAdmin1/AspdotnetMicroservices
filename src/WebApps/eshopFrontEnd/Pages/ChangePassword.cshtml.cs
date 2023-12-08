using eshopFrontEnd.Models;
using eshopFrontEnd.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eshopFrontEnd.Pages
{
    public class ChangePasswordModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        public ChangePasswordModel(IUserService userService, IConfiguration configuration)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public UserChangePasswordModel UserChangePasswordModel { get; set; }
        public void OnGet()
        {
            UserChangePasswordModel = new UserChangePasswordModel();
        }
        public async Task<IActionResult> OnPostChangeUserPassword(UserChangePasswordModel userChangePassword)
        {
            try
            {
                if (TryValidateModel(userChangePassword, nameof(UserChangePasswordModel)))
                {
                    var token = HttpContext.Session.Keys.Contains(_configuration["Session:SessionToken"]) ? HttpContext.Session.GetString(_configuration["Session:SessionToken"]) : throw new ArgumentNullException("Token Not Found");

                    var result = await _userService.ChangePassword(userChangePassword, token);
                    if (!string.IsNullOrEmpty(result.Item1))
                    {
                        return RedirectToPage("Login");
                    }
                    return RedirectToPage("error", result.Item2);
                }
            }
            catch (Exception ex)
            {
                return RedirectToPage("error", ex);
            }
            return Page();
        }
    }
}
