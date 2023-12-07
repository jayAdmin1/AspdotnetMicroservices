using eshopFrontEnd.Extensions;
using eshopFrontEnd.Models;
using eshopFrontEnd.Services.Interfaces;
using System.Net.Http.Headers;

namespace eshopFrontEnd.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _client;

        public UserService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }
        public UserErrorModel userError;
        public async Task<(UserAddModel, UserErrorModel)> CreateUser(UserAddModel userAddModel)
        {
            userError = new UserErrorModel();
            var reponse = await _client.PostAsJson($"/User", userAddModel);
            if (reponse.IsSuccessStatusCode)
            {
                userAddModel = await reponse.ReadContentAs<UserAddModel>();
                return (userAddModel, userError);
            }
            else if (reponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                //var res =  await reponse.Content.ReadFromJsonAsync<UserErrorModel>(); 
                userError = await reponse.Content.ReadFromJsonAsync<UserErrorModel>();
                return (userAddModel, userError);
            }
            else
            {
                throw new NotImplementedException("Something went wrong when calling api.");
            }
        }
        public async Task<(string, UserErrorModel)> Login(UserLoginModel userLoginModel)
        {
            userError = new UserErrorModel();
            var response = await _client.PostAsJson($"/User/Login", userLoginModel);
            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                return (token, userError);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                userError = await response.Content.ReadFromJsonAsync<UserErrorModel>();
                return (string.Empty, userError);
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
                return(result, userError);
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
    }
}
