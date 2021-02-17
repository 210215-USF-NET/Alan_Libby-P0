namespace StoreModels
{

    /// <summary>
    /// This data structure models a product and its quantity. The quantity was separated from the product as it could vary from orders and locations.  
    /// </summary>
    public class Item
    {
        public Product Product { get; set; }

        public int Quantity { get; set; }

        public Item copy() {
            Item item = new Item();
            item.Product = Product;
            item.Quantity = Quantity;
            return item;
        }
    }
}