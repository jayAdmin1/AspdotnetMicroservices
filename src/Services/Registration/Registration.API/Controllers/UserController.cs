using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Registration.API.Services.Abstration;
using Registration.API.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Registration.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserAddModel addUser, CancellationToken cancellationToken = default)
        {
            var result = await _userService.CreateUser(addUser, cancellationToken);
            return result.Item1 is not NullUserDisplayModel ? Ok(result.Item1) : BadRequest(result.Item2);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken = default)
        {
            var result = await _userService.GetAllUsers(cancellationToken);
            return result.Item1 is not NullUserListDisplayModel ? Ok(result.Item1) : BadRequest(result.Item2);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById([Required] Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _userService.GetUserById(id, cancellationToken);
            return result.Item1 is not NullUserDisplayModel ? Ok(result.Item1) : BadRequest(result.Item2);
        }

        [Authorize]
        [HttpDelete("{id}")]
        //[SwaggerResponse(StatusCodes.Status200OK)]
        //[SwaggerResponse(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveUser([Required] Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _userService.RemoveUser(id, cancellationToken);
            return !string.IsNullOrEmpty(result.Item1) ? Ok(result.Item1) : BadRequest(result.Item2);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser([Required] Guid id, UserUpdateModel userUpdateModel, CancellationToken cancellationToken = default)
        {
            var result = await _userService.UpdateUser(id, userUpdateModel, cancellationToken);
            return result.Item1 is not NullUserDisplayModel ? Ok(result.Item1) : BadRequest(result.Item2);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginModel userLogin, CancellationToken cancellationToken = default)
        {
            var result = await _userService.Login(userLogin, cancellationToken);
            return !string.IsNullOrEmpty(result.Item1) ? Ok(result.Item1) : BadRequest(result.Item2);
        }

        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(UserPasswordModel userPasswordModel, CancellationToken cancellationToken = default)
        {
            var result = await _userService.ChangePassword(userPasswordModel, cancellationToken);
            return !string.IsNullOrEmpty(result.Item1) ? Ok(result.Item1) : BadRequest(result.Item2);
        }

        [Authorize]
        [HttpPost("ChangeEmailAddress")]
        public async Task<IActionResult> ChangeEmailAddress(UserEmailModel userEmailModel ,CancellationToken cancellationToken = default)
        {
            var result = await _userService.ChangeEmail(userEmailModel, cancellationToken);
            return !string.IsNullOrEmpty(result.Item1) ? Ok(result.Item1) : BadRequest(result.Item2);
        }
    }
}
