using Store.Service.Services.basketService.CustomerBasketDto;

namespace Store.Service.Services.BasketServiceDtos
{
    public interface IBasketService
    {
        Task<CustomerBasketDto> GetBasketAsync(string basketId);
        Task<CustomerBasketDto> UpdateBasketAsync(CustomerBasketDto basket);
        Task<bool> DeleteBasketAsync(string basketId);

    }
}
