using System.Collections.Generic;

namespace StoreModels
{
    /// <summary>
    /// This class should contain all the fields and properties that define a customer order. 
    /// </summary>
    public class Order
    {
        public Customer Customer { get; set; }
        public Location Location { get; set; }
        public List<Item> Items { get; set; }
        public bool CheckedOut { get; set; }

        public Order() {
            this.Items = new List<Item>();
        }
        public Order(Customer customer, Location location, List<Item> items, bool checkedOut) {
            Customer = customer;
            Location = location;
            Items = items;
            CheckedOut = checkedOut;
        }
        public Order copy() {
            List<Item> newList = new List<Item>();
            foreach (Item item in Items) {
                newList.Add(item.copy());
            }
            return new Order(Customer, Location, newList, false);
        }
    }
}