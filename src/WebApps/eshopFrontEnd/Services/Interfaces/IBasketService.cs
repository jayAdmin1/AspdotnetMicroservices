using eshopFrontEnd.Models;

namespace eshopFrontEnd.Services.Interfaces
{
    public interface IBasketService
    {
        Task<BasketModel> GetBasket(string userName);
        Task<BasketModel> UpdateBasket(BasketModel basketModel);
        Task CheckoutBasket(BasketCheckoutModel basketCheckoutModel);
    }
}
