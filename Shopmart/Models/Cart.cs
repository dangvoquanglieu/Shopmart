using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopmart.Models
{
    public class Cart
    {
        public string CustomerName { get; set; }
        public Dictionary<string, OrderDetail> CartProduct { get; set; }

        public Cart()
        {

        }

        public Cart(string customerName, Dictionary<string, OrderDetail> cartProduct)
        {
            CustomerName = customerName;
            CartProduct = cartProduct;
        }

        public void add(OrderDetail orderDetail)
        {
            if (CartProduct == null)
            {
                this.CartProduct = new Dictionary<string, OrderDetail>();
            }
            string key = orderDetail.Product.ProductID;
            if (this.CartProduct.ContainsKey(key))
            {
                
                int quantity = this.CartProduct.GetValueOrDefault(key).Quantity;
                this.CartProduct.GetValueOrDefault(key).Quantity = quantity + 1;
            }
            else 
            { 
            this.CartProduct.Add(key, orderDetail);
            }

        }
    }
}
