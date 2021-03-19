using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopmart.Models
{
    public class OrderDetail
    {
        public string OrderDetailID { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public float TotalPrice { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
