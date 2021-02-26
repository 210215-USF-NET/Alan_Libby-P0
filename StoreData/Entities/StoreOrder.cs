using System;
using System.Collections.Generic;

#nullable disable

namespace StoreData.Entities
{
    public partial class StoreOrder
    {
        public StoreOrder()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int? LocationId { get; set; }
        public DateTime? CheckedOut { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Location Location { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
