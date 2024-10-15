using AutoMapper;
using StackExchange.Redis;
using Store.Data.Entities;
using Store.Data.Entities.OrderEntity;
using Store.Repository.Interfaces;
using Store.Repository.Specification.OrderSpecs;
using Store.Service.PaymentService;
using Store.Service.Services.BasketServiceDtos;
using Store.Service.Services.OrderService.Dtos;
using Stripe;
using Order = Store.Data.Entities.OrderEntity.Order;
using Product = Store.Data.Entities.Product;

namespace Store.Service.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IBasketService _basketService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;

        public OrderService(IBasketService basketService, IUnitOfWork unitOfWork, IMapper mapper ,IPaymentService paymentService)
        {
            _basketService = basketService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
          _paymentService = paymentService;
        }

        public async Task<OrderDetailsDto> CreateOrderAsync(OrderDto input)
        {
            #region 1 - التحقق من السلة
            var basket = await _basketService.GetBasketAsync(input.BasketId);
            if (basket == null)
                throw new Exception("Basket Not Exist");

            var orderItems = new List<OrderItemDto>();

            foreach (var basketItem in basket.BasketItems)
            {
                var productItem = await _unitOfWork.Repository<Product, int>()
                    .GetByIdAsync(basketItem.ProductId);

                if (productItem == null)
                    throw new Exception($"Product With id: {basketItem.ProductId} Not Exist");

                var itemOrdered = new PrdocutItem
                {
                    ProductId = productItem.Id,
                    PriductName = productItem.Name,
                    PictureUrl = productItem.PictureUrl
                };

                var orderItem = new OrderItem
                {
                    Price = productItem.Price,
                    Quantity = basketItem.Quantity,
                    prdocutItem = itemOrdered
                };

                var mappedOrderItem = _mapper.Map<OrderItemDto>(orderItem);
                orderItems.Add(mappedOrderItem);
            }
            #endregion

            #region 2 - التحقق من طريقة التوصيل
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod, int>()
                .GetByIdAsync(input.DeliveryMehtodId);

            if (deliveryMethod == null)
                throw new Exception("Delivery Method Not Provided");
            #endregion

            #region 3 - حساب المجموع الفرعي
            var subtotal = orderItems.Sum(item => item.Quantity * item.Price);
            #endregion
            #region PaymentMethod
            var specs = new OrderWithPaymentIntentSpecifications(basket.PaymentIntentId);
            var exixting = await _unitOfWork.Repository<Order, Guid>().GetWithSpecificationByIdAsync(specs);
            if(exixting == null)
            {
                await _paymentService.CreateOrUpdatePaymentIntent(basket);
            }

            #endregion


            // تحويل عنوان الشحن باستخدام AutoMapper
            var mappedShippingAddress = _mapper.Map<ShippingAddress>(input.ShippingAddress);

            // تحويل عناصر الطلب باستخدام AutoMapper
            var mappedOrderItems = _mapper.Map<List<OrderItem>>(orderItems);

            // إنشاء الطلب الجديد
            var order = new Order
            {
                DeliveryMethodId = deliveryMethod.Id,
                ShippingAddress = mappedShippingAddress,
                BasketId = input.BasketId,
                SubTotal = subtotal,
                PaymentIntentId=basket.PaymentIntentId
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
                throw new Exception("You do not have any orders yet");

            return _mapper.Map<List<OrderDetailsDto>>(orders);
        }

        public async Task<OrderDetailsDto> GetOrderByIdAsync(Guid id,string buyerEmail)
        {
            var specs = new OrderWithItemSpecifications(id ,buyerEmail);
            var order = await _unitOfWork.Repository<Order, Guid>().GetWithSpecificationByIdAsync(specs);

            if (order == null)
                throw new Exception($"There is no order with id: {id}");

            return _mapper.Map<OrderDetailsDto>(order);
        }
    }
}
