using eshopFrontEnd.Extensions;
using eshopFrontEnd.Models;
using eshopFrontEnd.Services.Interfaces;

namespace eshopFrontEnd.Services
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient _client;
        public BasketService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }
        public async Task<BasketModel> GetBasket(string userName)
        {
            var response = await _client.GetAsync($"/Basket/{userName}");
            return await response.ReadContentAs<BasketModel>();
        }

        public async Task<BasketModel> UpdateBasket(BasketModel basketModel)
        {
            var response = await _client.PostAsJson($"/Basket", basketModel);
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<BasketModel>();
            else
            {
                throw new NotImplementedException("Something went wrong when calling api.");
            }
        }
        public async Task CheckoutBasket(BasketCheckoutModel basketCheckoutModel)
        {
            var response = await _client.PostAsJson($"/Basket/Checkout", basketCheckoutModel);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Something went wrong when calling api.");
            }
        }

    }
}
