using eshopFrontEnd.Extensions;
using eshopFrontEnd.Models;
using eshopFrontEnd.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace eshopFrontEnd.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;

        public UserService(HttpClient client, IConfiguration configuration)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        public UserErrorModel userError;
        public async Task<(UserAddModel, UserErrorModel)> CreateUser(UserAddModel userAddModel)
        {
            userError = new UserErrorModel();
            var response = await _client.PostAsJson($"/User", userAddModel);
            if (response.IsSuccessStatusCode)
            {
                userAddModel = await response.ReadContentAs<UserAddModel>();
                return (userAddModel, userError);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                userError = await response.Content.ReadFromJsonAsync<UserErrorModel>();
                return (userAddModel, userError);
            }
            else
            {
                throw new NotImplementedException("Something went wrong when calling api.");
            }
        }
        public async Task<(string, Guid, UserErrorModel)> Login(UserLoginModel userLoginModel)
        {
            userError = new UserErrorModel();
            var response = await _client.PostAsJson($"/User/Login", userLoginModel);
            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                var userId = GetUserId(token);
                return (token, userId, userError);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                userError = await response.Content.ReadFromJsonAsync<UserErrorModel>();
                return (string.Empty, Guid.Empty, userError);
            }
            else
            {
                throw new NotImplementedException("Something went wrong when calling api.");
            }
        }
        public async Task<(string, UserErrorModel)> ChangePassword(UserChangePasswordModel userChangePasswordModel, string token)
        {
            userError = new UserErrorModel();
            if (string.IsNullOrEmpty(token))
            {
                userError.Message = "Token Not Found Please Login";
                return (string.Empty, userError);
            }
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.PostAsJsonAsync($"/User/ChangePassword", userChangePasswordModel);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return (result, userError);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                userError = await response.Content.ReadFromJsonAsync<UserErrorModel>();
                return (string.Empty, userError);
            }
            else
            {
                throw new NotImplementedException("Something went wrong when calling ChangePassword api.");
            }
        }

        public async Task<(string, UserErrorModel)> ChangeEmailAddress(UserChangeEmailAddressModel userChangeEmailAddressModel, string token)
        {
            userError = new UserErrorModel();
            if (string.IsNullOrEmpty(token))
            {
                userError.Message = "Token Not Found Please Login";
                return (string.Empty, userError);
            }
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.PostAsJsonAsync($"/User/ChangeEmailAddress", userChangeEmailAddressModel);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return (result, userError);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                userError = await response.Content.ReadFromJsonAsync<UserErrorModel>();
                return (string.Empty, userError);
            }
            else
            {
                throw new NotImplementedException("Something went wrong when calling ChangeEmailAddress api.");
            }
        }

        private Guid GetUserId(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var SecretKey = _configuration["JwtSecretKey"];
            var key = Encoding.ASCII.GetBytes(SecretKey);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key)
        ,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);
            var jwtToken = (JwtSecurityToken)validatedToken;
            return Guid.Parse(jwtToken.Claims.First(x => x.Type == "sub").Value);
        }

        public async Task<(UserAddModel, UserErrorModel)> UpdateUser(UserUpdateModel userUpdate,Guid userId, string token)
        {
            userError = new UserErrorModel();
            var userUpdated = new UserAddModel();
            if (string.IsNullOrEmpty(token))
            {
                userError.Message = "Token Not Found Please Login";
                return (userUpdated, userError);
            }
            if (userId == Guid.Empty)
            {
                userError.Message = "UserId Not Found Please Login";
                return (userUpdated, userError);
            }
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.PutAsJsonAsync($"/User/{userId}", userUpdate);

            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    userUpdated = await response.ReadContentAs<UserAddModel>();
                    return (userUpdated, userError);
                case System.Net.HttpStatusCode.BadRequest:
                    userError = await response.Content.ReadFromJsonAsync<UserErrorModel>();
                    return (userUpdated, userError);
                case System.Net.HttpStatusCode.NotFound:
                    userError = await response.Content.ReadFromJsonAsync<UserErrorModel>();
                    return (userUpdated, userError);
                default:
                    throw new NotImplementedException("Something went wrong when calling api.");
            }
        }
    }
}
