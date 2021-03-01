using System.Collections.Generic;
using System;
using StoreModels;

namespace StoreData
{
    internal struct InventoryTuple {
        public InventoryTuple(Location _loc, Product _prod) {
            this.Loc = _loc;
            this.Prod = _prod;
        }
        public Location Loc { get; set; }
        public Product Prod { get; set; }
    }
    public class MemoryDataStore : IDataStore
    {
        private Dictionary<string, Customer> customers;
        private Dictionary<InventoryTuple, int> inventory;
        private List<Location> locations;
        private List<Product> products;
        private List<Order> orders;
        public MemoryDataStore()
        {
            customers = new Dictionary<string, Customer>();
            inventory = new Dictionary<InventoryTuple, int>();
            locations = new List<Location>();
            products = new List<Product>();
            orders = new List<Order>();
            // TODO: insert default values

            locations.Add(new Location());
            locations[0].LocationName = "Test Location Even";

            locations.Add(new Location());
            locations[1].LocationName = "Test Location Odd";

            locations.Add(new Location());
            locations[2].LocationName = "Test Location All";

            for (int i = 0; i < 5; ++i) {
                Product product = new Product();
                product.ProductName = "Product " + i;
                product.Price = i + (decimal)0.99;
                products.Add(product);

                inventory.Add(new InventoryTuple(locations[i % 2], product), i * 100 + 50);
                inventory.Add(new InventoryTuple(locations[2], product), 100);
            }
        }

        public List<Location> GetAvailableLocations(Product product) {
            List<Location> x = new List<Location>();
            foreach(Location loc in locations) {
                if (inventory.ContainsKey(new InventoryTuple(loc, product))) {
                    x.Add(loc);
                }
            }
            return x;
        }

        public List<Product> GetAvailableProducts(Location location) {
            List<Product> x = new List<Product>();
            foreach(Product prod in products) {
                if (inventory.ContainsKey(new InventoryTuple(location, prod))) {
                    x.Add(prod);
                }
            }
            return x;
        }

        public int GetLocationInventory(Location location, Product product) {
            return inventory[new InventoryTuple(location, product)];
        }

        public Customer GetCustomer(string name) {
            Customer customer = null;
            if (customers.TryGetValue(name, out customer))
                return customer;
            return null;
        }

        public int? AddCustomer(string name, Customer customer) {
            if (customers.ContainsKey(name))
                return null;
            customers.Add(name, customer);
            return 0;
        }

        public List<Product> GetAllProducts() {
            return products;
        }

        public void UpdateLocationInventory(Location location, Product product, int delta) {
            InventoryTuple tuple = new InventoryTuple(location, product);
            int i;
            inventory.TryGetValue(tuple, out i);
            i += delta;
            if (i < 0)
                throw new Exception("Tried to set inventory quantity to " + i);
            else if (i == 0) 
                inventory.Remove(tuple);
            else
                inventory[tuple] = i;
        }

        public void PlaceOrder(Order order) {
            orders.Add(order);
        }

        public List<Order> GetCustomerOrders(Customer customer) {
            List<Order> x = new List<Order>();
            foreach(Order order in orders) {
                if (order.Customer == customer) {
                    x.Add(order);
                }
            }
            return x;
        }

        public List<Order> GetAllOrders() {
            return orders;
        }
    }
}