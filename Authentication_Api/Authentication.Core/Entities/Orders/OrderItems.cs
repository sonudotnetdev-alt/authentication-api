namespace Authentication.Core.Entities.Orders
{
    public class OrderItems:BasicEntity<int>
    {
        public OrderItems()
        {
        }

        public OrderItems(ProductItemOrdered productItemOrdered, decimal price, decimal? quantity, decimal? priceDiscount)
        {
            this.productItemOrdered = productItemOrdered;
            this.price = price;
            this.quantity = quantity;
            this.priceDiscount = priceDiscount;
        }
        public int OrderItemId { get; set; }
        public ProductItemOrdered productItemOrdered { get; set; }
        public decimal price { get; set; }
        public decimal? quantity { get; set; } = default(decimal?);
        public decimal? priceDiscount { get; set; }
    }
}