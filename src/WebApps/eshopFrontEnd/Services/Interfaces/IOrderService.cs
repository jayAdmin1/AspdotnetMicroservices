using eshopFrontEnd.Models;

namespace eshopFrontEnd.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponseModel>> GetOrdersByUserName(string userName);
    }
}
