using AutoMapper;
using Store.Repository.Basket;
using Store.Repository.Basket.Models;
using Store.Service.Services.basketService.CustomerBasketDto;
using Store.Service.Services.BasketServiceDtos;

namespace Store.Service.Services.basketService
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketService(IBasketRepository basketRepository ,IMapper mapper) 
        {
          _basketRepository = basketRepository;
          _mapper = mapper;
        }
        public async Task<bool> DeleteBasketAsync(string basketId)
       =>await _basketRepository.DeleteBasketAsync(basketId);

        public async Task<CustomerBasketDto.CustomerBasketDto> GetBasketAsync(string basketId)
        {
           var basket =await _basketRepository.GEtBasketAsync(basketId);
            if (basket == null)
            {
                return new CustomerBasketDto.CustomerBasketDto();
            }
            var mappedBasket=_mapper.Map<CustomerBasketDto.CustomerBasketDto> (basket);
          return mappedBasket;
        }

        public async Task<CustomerBasketDto.CustomerBasketDto> UpdateBasketAsync(CustomerBasketDto.CustomerBasketDto input)
        {
            if (input.Id is null)
            {
                input.Id = Genreate();
            }
            var customerbasket = _mapper.Map<CustomerBasket>(input);
            var updateBasket =await _basketRepository .UpdateBasketAsync(customerbasket);

            var mappedUpdateBasket=_mapper.Map<CustomerBasketDto.CustomerBasketDto>(updateBasket);

            return mappedUpdateBasket;
        }
        private string Genreate()
        {
            Random random = new Random();
            int randomdigit = random.Next(1000, 10000);
            return $"BS-{randomdigit}";

        }
    }
}
