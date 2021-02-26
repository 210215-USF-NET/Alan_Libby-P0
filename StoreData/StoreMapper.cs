using Models = StoreModels;
using Entities = StoreData.Entities;
using System.Collections.Generic;
using System;

namespace StoreData
{
    public class StoreMapper
    {
        public Models.Customer ParseCustomer(Entities.Customer customer) {
            Models.Customer c = new Models.Customer();
            c.CustomerID = customer.CustomerId;
            c.Name = customer.CustomerName;
            return c;
        }
        public Entities.Customer ParseCustomer(Models.Customer customer) {
            Entities.Customer c = new Entities.Customer();
            if (customer.CustomerID != null) c.CustomerId = (int)customer.CustomerID;
            c.CustomerName = customer.Name;
            return c;
        }
        // public Models.Inventory ParseInventory(Entities.Inventory inventory) {
        //     return new Models.Inventory();
        // }
        // public Entities.Inventory ParseInventory(Models.Inventory inventory) {
        //     return new Entities.Inventory();
        // }
        public Models.Location ParseLocation(Entities.Location location) {
            Models.Location loc = new Models.Location();
            loc.LocationID = location.LocationId;
            loc.LocationName = location.LocationName;
            loc.Address = location.LocationAddress;
            return loc;
        }
        public Entities.Location ParseLocation(Models.Location location) {
            Entities.Location loc = new Entities.Location();
            if (location.LocationID != null) loc.LocationId = (int)location.LocationID;
            loc.LocationName = location.LocationName;
            loc.LocationAddress = location.Address;
            return loc;
        }
        public Models.Item ParseItem(Entities.OrderItem item) {
            return new Models.Item();
        }
        public Entities.OrderItem ParseItem(Models.Item item) {
            return new Entities.OrderItem();
        }
        public Models.Product ParseProduct(Entities.Product product) {
            Models.Product p = new Models.Product();
            p.ProductID = product.ProductId;
            p.ProductName = product.ProductName;
            p.Price = product.ProductPrice;
            return p;
        }
        public Entities.Product ParseProduct(Models.Product product) {
            Entities.Product p = new Entities.Product();
            if (product.ProductID != null) p.ProductId = (int)product.ProductID;
            p.ProductName = product.ProductName;
            p.ProductPrice = product.Price;
            return p;
        }
        public Models.Order ParseOrder(Entities.StoreOrder order) {
            Models.Order o = new Models.Order();
            o.OrderID = order.OrderId;
            o.Customer = ParseCustomer(order.Customer);
            o.Location = ParseLocation(order.Location);
            o.CheckedOut = order.CheckedOut != null;
            if (o.CheckedOut) o.CheckoutTimestamp = (DateTime)order.CheckedOut;
            List<Models.Item> items = new List<Models.Item>();
            foreach(Entities.OrderItem item in order.OrderItems) {
                Models.Item i = new Models.Item();
                i.Product = ParseProduct(item.Product);
                i.Quantity = item.Quantity;
                items.Add(i);
            }
            o.Items = items;
            return o;
        }
        public Entities.StoreOrder ParseOrder(Models.Order order) {
            return new Entities.StoreOrder();
        }
    }
}