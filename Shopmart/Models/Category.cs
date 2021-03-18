using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopmart.Models
{
    public class Category
    {
        public string CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Status { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
