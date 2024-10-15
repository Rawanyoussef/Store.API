using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.OrderService.Dtos
{
    public class OrderDto
    {
        public string BasketId { get; set; }
        public string BasketEmail { get; set; }
        [Required]
        public int DeliveryMehtodId  { get; set; }
        public AddressDto ShippingAddress { get; set; }
        public decimal ShippingPrice { get; set; }
    }
}
