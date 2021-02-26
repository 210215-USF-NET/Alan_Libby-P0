using Models = StoreModels;
using Entities = StoreData.Entities;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;

namespace StoreData
{
    public class DatabaseDataStore : IDataStore
    {
        protected Entities.storeContext ctx;
        protected StoreMapper mapper;
        public DatabaseDataStore(Entities.storeContext ctx) {
            this.ctx = ctx;
            mapper = new StoreMapper();
        }
        public Models.Customer GetCustomer(string name) {
            return ctx.Customers.Select(c => mapper.ParseCustomer(c)).ToList().FirstOrDefault(c => c.Name == name);
        }
        public int? AddCustomer(string name, Models.Customer customer) {
            Entities.Customer cEntity = mapper.ParseCustomer(customer);
            ctx.Customers.Add(cEntity);
            ctx.SaveChanges();
            return cEntity.CustomerId;
        }
        public List<Models.Product> GetAllProducts() {
            return ctx.Products.Select(p => mapper.ParseProduct(p)).ToList();
        }
        public List<Models.Location> GetAvailableLocations(Models.Product product) {
            return ctx.Inventories.Include(i => i.Location).Where(i => i.ProductId == product.ProductID).Select(i => mapper.ParseLocation(i.Location)).ToList();
        }
        public List<Models.Product> GetAvailableProducts(Models.Location location) {
            return ctx.Inventories.Include(i => i.Product).Where(i => i.LocationId == location.LocationID).Select(i => mapper.ParseProduct(i.Product)).ToList();
        }
        public int GetLocationInventory(Models.Location location, Models.Product product) {
            return ctx.Inventories.Where(i => i.LocationId == location.LocationID && i.ProductId == product.ProductID).Select(i => i.Quantity).ToList().FirstOrDefault();
        }
        public void UpdateLocationInventory(Models.Location location, Models.Product product, int delta) {
            Entities.Inventory i = ctx.Inventories.Where(i => i.LocationId == location.LocationID && i.ProductId == product.ProductID).ToList().FirstOrDefault();
            if (i == null) {
                i = new Entities.Inventory();
                if (location.LocationID != null) i.LocationId = (int)location.LocationID;
                if (product.ProductID != null) i.ProductId = (int)product.ProductID;
                i.Quantity = 0;
                ctx.Inventories.Add(i);
            }
            i.Quantity += delta;
            if (i.Quantity < 0) {
                throw new Exception("Tried to set inventory quantity to " + i.Quantity);
            } else if (i.Quantity == 0) {
                ctx.Inventories.Remove(i);
            }
            ctx.SaveChanges(); // TODO: find a way to avoid this
        }
        public void PlaceOrder(Models.Order order) {
            Entities.StoreOrder o = new Entities.StoreOrder();
            if (order.Location.LocationID != null) o.LocationId = (int)order.Location.LocationID;
            if (order.Customer.CustomerID != null) o.CustomerId = (int)order.Customer.CustomerID;
            o.CheckedOut = DateTime.Now;
            ctx.StoreOrders.Add(o);
            foreach(Models.Item i in order.Items) {
                Entities.OrderItem oi = new Entities.OrderItem();
                if (i.Product.ProductID != null) oi.ProductId = (int)i.Product.ProductID;
                oi.Quantity = i.Quantity;
                o.OrderItems.Add(oi);
            }
            ctx.SaveChanges();
        }
        public List<Models.Order> GetCustomerOrders(Models.Customer customer) {
            return ctx.StoreOrders.Include(o => o.Customer).Include(o => o.Location).Include(o => o.OrderItems).ThenInclude(i => i.Product).Where(o => o.CustomerId == customer.CustomerID).Select(o => mapper.ParseOrder(o)).ToList();
        }
    }
}