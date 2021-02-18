using System.Collections.Generic;
using System;
using StoreModels;

namespace StoreData
{
    public class MemoryDataStore : IDataStore
    {
        private Dictionary<string, Customer> customers;
        private List<Product> products;
        public MemoryDataStore()
        {
            customers = new Dictionary<string, Customer>();
            products = new List<Product>();
            // TODO: insert default values
            for (int i = 0; i < 5; ++i) {
                Product product = new Product();
                product.ProductName = "Product " + i;
                product.Price = i + 0.99;
                products.Add(product);
            }
        }
        public Customer GetCustomer(string name) {
            Customer customer = null;
            if (customers.TryGetValue(name, out customer))
                return customer;
            return null;
        }

        public bool AddCustomer(string name, Customer customer) {
            if (customers.ContainsKey(name))
                return false;
            customers.Add(name, customer);
            return true;
        }

        public List<Product> GetProducts() {
            return products;
        }
    }
}