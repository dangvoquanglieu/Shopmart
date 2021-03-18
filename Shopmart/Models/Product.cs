using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopmart.Models
{
    public class Product
    {
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public string UrlImage { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int Quantity { get; set; }
        public string CategoryID { get; set; }
        public Category Category { get; set; }
    }
}
