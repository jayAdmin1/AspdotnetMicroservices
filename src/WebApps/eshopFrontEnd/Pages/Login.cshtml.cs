using eshopFrontEnd.Models;
using eshopFrontEnd.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eshopFrontEnd.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        public LoginModel(IUserService userService, IConfiguration configuration)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public UserLoginModel userLogin { get; set; }
        public void OnGet()
        {
            userLogin = new UserLoginModel();
        }
        public async Task<IActionResult> OnPostLoginUser(UserLoginModel userLoginModel)
        {
            if (TryValidateModel(userLoginModel, nameof(userLogin)))
            {
                var result = await _userService.Login(userLoginModel);
                if (!string.IsNullOrEmpty(result.Item1) && (result.Item2 != Guid.Empty))
                {
                    HttpContext.Session.SetString(_configuration["Session:SessionEmail"], userLoginModel.EmailAddress);
                    HttpContext.Session.SetString(_configuration["Session:SessionToken"], result.Item1);
                    HttpContext.Session.SetString(_configuration["Session:SessionUserId"], result.Item2.ToString());
                    return RedirectToPage("Index");
                }
                return RedirectToPage("error", result.Item3);
            }
           return Page();
        }
    }
}
