using System.Diagnostics;
using eshopFrontEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eshopFrontEnd.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorModel : PageModel
    {
        public string RequestId { get; set; }
        public string StatusMessage { get; set; } = string.Empty;

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        private readonly ILogger<ErrorModel> _logger;

        public ErrorModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }

        //public void OnGet()
        //{
        //    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        //}
        public void OnGet(string Message)
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            StatusMessage = Message;
        }
    }
}
