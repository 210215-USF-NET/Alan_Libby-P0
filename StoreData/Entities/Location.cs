﻿using System;
using System.Collections.Generic;

#nullable disable

namespace StoreData.Entities
{
    public partial class Location
    {
        public Location()
        {
            Inventories = new HashSet<Inventory>();
            StoreOrders = new HashSet<StoreOrder>();
        }

        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string LocationAddress { get; set; }

        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<StoreOrder> StoreOrders { get; set; }
    }
}
