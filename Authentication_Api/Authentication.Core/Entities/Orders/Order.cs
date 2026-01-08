using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Core.Entities.Orders
{
    public class Order : BasicEntity<int>
    {
        public Order() { }
        public Order(string buyerEmail, ShipAddress shipAddress, DeliveryMethod deliveryMethod, IReadOnlyList<OrderItems> orderItems, decimal subTotal, string paymentIntentId)
        {
            BuyerEmail = buyerEmail;
            this.shipAddress = shipAddress;
            this.deliveryMethod = deliveryMethod;
            this.orderItems = orderItems;
            SubTotal = subTotal;
            PaymentIntentId = paymentIntentId;
        }
        public int OrderId { get; set; }
        public string BuyerEmail { get; set; }
        public DateTime? OrderDate { get; set; } = DateTime.Now;
        public ShipAddress shipAddress { get; set; }
        public DeliveryMethod deliveryMethod { get; set; }
        public IReadOnlyList<OrderItems> orderItems { get; set; }
        public decimal SubTotal { get; set; }
        public OrderStatus orderStatus { get; set; } = OrderStatus.pending;
        public string PaymentIntentId { get; set; }
        public decimal GetTotal()
        {
            return SubTotal + deliveryMethod.Price;
        }
    }
}
