using System;
using System.Collections.Generic;
using StoreModels;

namespace StoreData
{
    public interface IDataStore
    {
        public Customer GetCustomer(string name);
        public bool AddCustomer(string name, Customer customer);
        public List<Product> GetProducts();
    }
}
