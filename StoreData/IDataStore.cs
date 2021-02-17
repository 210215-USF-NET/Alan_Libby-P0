using System;
using StoreModels;

namespace StoreData
{
    public interface IDataStore
    {
        public Customer GetCustomer(string name);
        public bool AddCustomer(string name, Customer customer);
    }
}
