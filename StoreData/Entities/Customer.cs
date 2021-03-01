using System;
using System.Collections.Generic;

#nullable disable

namespace StoreData.Entities
{
    public partial class Customer
    {
        public Customer()
        {
            StoreOrders = new HashSet<StoreOrder>();
        }

        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public bool IsManager { get; set; }

        public virtual ICollection<StoreOrder> StoreOrders { get; set; }
    }
}
