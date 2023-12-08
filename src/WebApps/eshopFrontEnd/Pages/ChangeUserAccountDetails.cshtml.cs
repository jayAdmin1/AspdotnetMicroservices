using eshopFrontEnd.Models;
using eshopFrontEnd.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eshopFrontEnd.Pages
{
    public class ChangeUserAccountDetailsModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public ChangeUserAccountDetailsModel(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public UserUpdateModel userUpdateModel { get; set; }
        public void OnGet()
        {
            userUpdateModel = new UserUpdateModel();
        }
        public async Task<IActionResult> OnPostUpdateUser(UserUpdateModel updateUserModel)
        {
            try
            {
                if (TryValidateModel(updateUserModel, nameof(userUpdateModel)))
                {
                    var token = HttpContext.Session.Keys.Contains(_configuration["Session:SessionToken"]) ? HttpContext.Session.GetString(_configuration["Session:SessionToken"]) : throw new ArgumentNullException("Token Not Found. Please Login");

                    var userId = HttpContext.Session.Keys.Contains(_configuration["Session:SessionUserId"]) ? Guid.Parse(HttpContext.Session.GetString(_configuration["Session:SessionUserId"])) : throw new ArgumentNullException("UserId Not Found");
                    
                    var result = await _userService.UpdateUser(updateUserModel, userId, token);
                    if (!string.IsNullOrEmpty(result.Item2.Message))
                    {
                        return RedirectToPage("error", result.Item2);
                    }
                    return RedirectToPage("Login");
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
