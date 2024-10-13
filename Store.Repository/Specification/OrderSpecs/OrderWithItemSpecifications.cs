using Store.Data.Entities.OrderEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specification.OrderSpecs
{
    public class OrderWithItemSpecifications : BaseSpecification<Order>
    {
        public OrderWithItemSpecifications(string buyerEmail)
            : base(order => order.BuyerEmail == buyerEmail)
        {
            AddInclude(order => order.DeliveryMethod);
            AddInclude(order => order.Items);
            AddOrderByDescending(order => order.orderDate);
        }

        public OrderWithItemSpecifications(Guid id)
           : base(order => order.Id == id)
        {
            AddInclude(order => order.DeliveryMethod);
            AddInclude(order => order.Items);
        }
    }
}
