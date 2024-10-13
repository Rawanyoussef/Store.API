using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data.Entities.OrderEntity
{
    public class Order :BaseEntity<Guid>
    {
        public string BuyerEmail { get; set; }
        public DateTimeOffset orderDate { get; set; }
        public ShippingAddress ShippingAddress  { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public int? DeliveryMethodId { get; set; }
        public OrderStauts OrderStauts { get; set; } = OrderStauts.Placed;
        public OrderPaymentStatus OrderPaymentStatus { get; set; } =OrderPaymentStatus.pending;
        public IReadOnlyCollection<OrderItem> Items { get; set;}

        public string?BasketId { get; set; }
        public decimal? SubTotal { get; set; }
        public decimal GetTotal()
            => (decimal)(SubTotal + DeliveryMethod.Price);
        public string? PaymentIntentId {  get; set; } 

    }
}
