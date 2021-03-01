using System;
using System.Collections.Generic;
using StoreModels;

namespace StoreData
{
    public interface IDataStore
    {
        Customer GetCustomer(string name);
        int? AddCustomer(string name, Customer customer);
        List<Product> GetAllProducts();
        List<Location> GetAvailableLocations(Product product);
        List<Product> GetAvailableProducts(Location location);
        int GetLocationInventory(Location location, Product product);
        void UpdateLocationInventory(Location location, Product product, int delta);
        void PlaceOrder(Order order);
        List<Order> GetCustomerOrders(Customer customer);
        List<Order> GetAllOrders();
    }
}
