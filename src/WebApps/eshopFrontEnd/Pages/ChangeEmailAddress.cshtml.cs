using eshopFrontEnd.Models;
using eshopFrontEnd.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eshopFrontEnd.Pages
{
    public class ChangeEmailAddressModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        public ChangeEmailAddressModel(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public UserChangeEmailAddressModel UserChangeEmailAddressModel { get; set; }
        public void OnGet()
        {
            UserChangeEmailAddressModel = new UserChangeEmailAddressModel();
        }
        public async Task<IActionResult> OnPostChangeUserEmail(UserChangeEmailAddressModel userChangeEmailAddress)
        {
            try
            {
                if (TryValidateModel(userChangeEmailAddress, nameof(UserChangeEmailAddressModel)))
                {
                    var token = HttpContext.Session.Keys.Contains(_configuration["Session:SessionToken"]) ? HttpContext.Session.GetString(_configuration["Session:SessionToken"]) : throw new ArgumentNullException("Token Not Found");

                    var result = await _userService.ChangeEmailAddress(userChangeEmailAddress, token);
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
