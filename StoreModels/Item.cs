namespace StoreModels
{

    /// <summary>
    /// This data structure models a product and its quantity. The quantity was separated from the product as it could vary from orders and locations.  
    /// </summary>
    public class Item
    {
        public int? ItemID { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }

        public Item copy() {
            Item item = new Item();
            item.Product = Product;
            item.Quantity = Quantity;
            return item;
        }

        public decimal Total { get {
            if (this.Product == null) return 0;
            return this.Product.Price * this.Quantity;
        } }
    }
}