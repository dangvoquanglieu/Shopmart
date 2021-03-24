using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopmart.Models
{
    public class Order
    {
        public string OrderID { get; set; }
        public string Id { get; set; }
        public ConfigUser User { get; set; }
        public DateTime CreateDate { get; set; }
        public float TotalPrice { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
