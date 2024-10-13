using AutoMapper;
using Store.Data.Entities;
using Store.Data.Entities.OrderEntity;
using Store.Repository.Interfaces;
using Store.Repository.Specification.OrderSpecs;
using Store.Service.Services.BasketServiceDtos;
using Store.Service.Services.OrderService.Dtos;

namespace Store.Service.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IBasketService _basketService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IBasketService basketService
            , IUnitOfWork unitOfWork, IMapper mapper)
        {
            _basketService = basketService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<OrderDetailsDto> CreateOrderAsync(OrderDto input)
        {
            #region 1
            var basket = await _basketService.GetBasketAsync(input.BasketId);
            if (basket == null)
                throw new Exception("Basket Not Exist");
            var orderItem = new List<OrderItemDto>();
            foreach (var basketItems in basket.BasketItems)
            {
                var ProductItem = await _unitOfWork.Repository<Product, int>().GetByIdAsync(basketItems.ProductId);
                if (ProductItem is null)
                    throw new Exception($"Product With id : {basketItems.ProductId} Not Exist");
                var itemOrdered = new PrdocutItem
                {
                    ProductId = ProductItem.Id,
                    PriductName = ProductItem.Name,
                    PictureUrl = ProductItem.PictureUrl,
                };

                var orderItem1 = new OrderItem
                {
                    Price = ProductItem.Price,
                    Quantity = basketItems.Quantity,
                    prdocutItem = itemOrdered
                };

                var mappedOrderItem = _mapper.Map<OrderItemDto>(orderItem);

                orderItem.Add(mappedOrderItem);
                #endregion
                #region 2

                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod, int>().GetByIdAsync(input.DeliveryMehtod);

                if (deliveryMethod is null)
                    throw new Exception("Delivery Method Not Provided");


                #endregion
                #region 3
                var subtotal = orderItem.Sum(item => item.Quantity * item.Price);
                #endregion
            }
            var mappedShippingAddress = _mapper.Map<ShippingAddress>(input.ShippingAddress);

            var mappedOrderItems = _mapper.Map<List<OrderItem>>(orderItem);

            var order = new Order
            {
                DeliveryMethodId = DeliveryMethod.Id,
                ShippingAddress = mappedShippingAddress,
                BasketId = input.BasketId,
                SubTotal = input.subtotal,
            };

            await _unitOfWork.Repository<Order, Guid>().AddAsync(order);

            await _unitOfWork.CompleteAsync();

            var mappedOrder = _mapper.Map<OrderDetailsDto>(order);



            return mappedOrder;

        }
        public async Task<IReadOnlyList<DeliveryMethod>> GetAllDeliveryMethodsAsync()
         => await _unitOfWork.Repository<DeliveryMethod, int>().GetAllAsync();

        public async Task<IReadOnlyList<OrderDetailsDto>> GetAllOrdersForUsersAsync(string buyerEmail)
        {
            var specs = new OrderWithItemSpecifications(buyerEmail);

            var orders = await _unitOfWork.Repository<Order, Guid>().GetAlltWithSpecificationAsync(specs);

            if (!orders.Any())
                throw new Exception("You Do not have any Orders yet");

            var mappedOrders = _mapper.Map<List<OrderDetailsDto>>(orders);

            return mappedOrders;
        }

        public async Task<OrderDetailsDto> GetOrderByIdAsync(Guid id)
        {
            var specs = new OrderWithItemSpecifications(id);

            var order = await _unitOfWork.Repository<Order, Guid>().GetWithSpecificationByIdAsync(specs);

            if (order is null)
                throw new Exception($"There is No Order With id : {id}");

            var mappedOrder = _mapper.Map<OrderDetailsDto>(order);

            return mappedOrder;
        }



    }
}

