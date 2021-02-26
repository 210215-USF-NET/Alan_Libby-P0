using System;
using System.Collections.Generic;

#nullable disable

namespace StoreData.Entities
{
    public partial class OrderItem
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public virtual StoreOrder Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
