using AutoMapper;
using Store.Data.Entities.OrderEntity;

namespace Store.Service.Services.OrderService.Dtos
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {

            CreateMap<ShippingAddress, AddressDto>().ReverseMap();
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.DeliveryMehtod,
                           options => options.MapFrom(src => src.DeliveryMethod.ShortName))
                .ForMember(dest => dest.ShippingPrice,
                           options => options.MapFrom(src => src.DeliveryMethod.Price));
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductItemId,
                           options => options.MapFrom(src => src.prdocutItem.ProductId))
                .ForMember(dest => dest.ProductName,
                           options => options.MapFrom(src => src.prdocutItem.PriductName))
                .ForMember(dest => dest.PictureUrl, options => options.MapFrom<OrderItemPictureUrlResolver>()).ReverseMap();
        }
    }
}