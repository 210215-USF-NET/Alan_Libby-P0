using System.Collections.Generic;
using System;
using StoreModels;

namespace StoreData
{
    public class MemoryDataStore : IDataStore
    {
        private Dictionary<string, Customer> customers;
        public MemoryDataStore()
        {
            customers = new Dictionary<string, Customer>();
            // TODO: insert default values
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
    }
}