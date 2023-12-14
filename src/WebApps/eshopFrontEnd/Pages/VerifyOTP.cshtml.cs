using eshopFrontEnd.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq.Expressions;
using static System.Net.WebRequestMethods;

namespace eshopFrontEnd.Pages
{
    public class VerifyOTPModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private static string _pageName;
        private static string _token;
        private static string _userEmailAddress;
        public VerifyOTPModel(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public async Task<IActionResult> OnGet(string pageName)
        {
            try
            {
                _pageName = pageName;
                _token = HttpContext.Session.Keys.Contains(_configuration["Session:SessionToken"]) ? HttpContext.Session.GetString(_configuration["Session:SessionToken"]) : throw new ArgumentNullException("Token Not Found");

                _userEmailAddress = HttpContext.Session.GetString(_configuration["Session:SessionEmail"]);
                //await _userService.SendOTP(_token, _userEmailAddress);

                return Page();
            }
            catch (Exception ex)
            {
                return RedirectToPage("error", ex);

            }
        }
        public async Task<IActionResult> OnPostVerifyOTP(string OTP)
        {
            try
            {
                string pageName = _pageName == null ? "OTPVerify" : _pageName;
                var result = await _userService.VerifyOTP(_token, _userEmailAddress, OTP);
                if (!string.IsNullOrEmpty(result.Item1))
                {
                    switch (pageName)
                    {
                        case "ChangePassword":
                            return RedirectToPage(pageName);
                        case "ChangeEmailAddress":
                            return RedirectToPage(pageName);
                        case "ChangeUserAccountDetails":
                            return RedirectToPage(pageName);
                        default:
                            return Page();
                    }
                }
                return RedirectToPage("error", result.Item2);
            }
            catch (Exception ex)
            {
                return RedirectToPage("error", ex);
            }
        }

        public async Task<IActionResult> OnGetResendOTP()
        {
            try
            {
                string pageName = _pageName == null ? "OTPVerify" : _pageName;
                await _userService.ResendOTP(_token, _userEmailAddress);
            }
            catch (Exception ex)
            {
                return RedirectToPage("error", ex);
            }
            return Page();
        }
    }
}
