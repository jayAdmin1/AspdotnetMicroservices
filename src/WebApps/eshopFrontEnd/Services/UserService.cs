using eshopFrontEnd.Extensions;
using eshopFrontEnd.Models;
using eshopFrontEnd.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

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
    }
}
