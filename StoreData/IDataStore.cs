using System;
using System.Collections.Generic;
using StoreModels;

namespace StoreData
{
    public interface IDataStore
    {
        Customer GetCustomer(string name);
        bool AddCustomer(string name, Customer customer);
        List<Product> GetProducts();
        List<Location> GetAvailableLocations(Product product);
        int GetLocationInventory(Location location, Product product);
    }
}
