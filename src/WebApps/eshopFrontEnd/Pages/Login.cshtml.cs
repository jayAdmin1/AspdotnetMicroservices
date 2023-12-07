using eshopFrontEnd.Models;
using eshopFrontEnd.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System.Runtime.InteropServices;

namespace eshopFrontEnd.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IUserService _userService;
        //private const string SessionEmail = "_Email";
        //private const string SessionToken = "_Token";
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
        public async Task<IActionResult> OnPostLoginUserAsync(UserLoginModel userLoginModel)
        {
            var userLogin = await _userService.Login(userLoginModel);
            if (!string.IsNullOrEmpty(userLogin.Item1))
            {
                HttpContext.Session.SetString(_configuration["Session:SessionEmail"], userLoginModel.EmailAddress);
                HttpContext.Session.SetString(_configuration["Session:SessionToken"],userLogin.Item1);
                return RedirectToPage("Index");
            }
            return RedirectToPage("error", userLogin.Item2);
        }
    }
}
