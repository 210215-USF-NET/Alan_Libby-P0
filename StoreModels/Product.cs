namespace StoreModels
{
    //This class should contain all necessary fields to define a product.
    public class Product
    {
        public int? ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        //todo: add more properties to define a product (maybe a category?)
        public override string ToString() {
            return ProductName + ": $" + Price;
        }
    }
}