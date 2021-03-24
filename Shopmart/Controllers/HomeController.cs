using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shopmart.Data;
using Shopmart.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Shopmart.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        private ApplicationDbContext db;
        private Cart cart;
        public HomeController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var listProduct = db.Products.Where(s => s.Status.Equals("true"));
            return View(listProduct);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Roles = "User")]
        public IActionResult Shopping(string id)
        {

            var key = HttpContext.Session.GetString("CART");
            var product = db.Products.Find(id);
            var orderDetail = new OrderDetail
            {
                Product = product,
                Quantity = 1,
            };
            if (key == null)
            {
                cart = new Cart();
            }
            else
            {
                cart = JsonConvert.DeserializeObject<Cart>(key);
            }
            cart.add(orderDetail);
            HttpContext.Session.SetString("CART", JsonConvert.SerializeObject(cart));
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "User")]
        public IActionResult ShoppingUpdate()
        {
            var key = HttpContext.Session.GetString("CART");
            cart = JsonConvert.DeserializeObject<Cart>(key);
            string productID = Request.Form["txtProductID"].ToString();
            int quantity = int.Parse(Request.Form["txtQuantity"].ToString());
            cart.update(productID, quantity);
            HttpContext.Session.SetString("CART", JsonConvert.SerializeObject(cart));
            return RedirectToAction(nameof(ShoppingView));
        }

        public IActionResult ShoppingDelete(string id)
        {
            var key = HttpContext.Session.GetString("CART");
            cart = JsonConvert.DeserializeObject<Cart>(key);
            cart.delete(id);
            HttpContext.Session.SetString("CART", JsonConvert.SerializeObject(cart));
            return RedirectToAction(nameof(ShoppingView));
        }

        public IActionResult ShoppingView()
        {
            var key = HttpContext.Session.GetString("CART");
            if (key != null)
            {
                cart = JsonConvert.DeserializeObject<Cart>(key);
            }
            return View(cart);
        }

        public IActionResult Checkout()
        {
            int count = 0;
            bool result = true;
            var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            float totalPrice = 0;
            var key = HttpContext.Session.GetString("CART");
            if (key != null)
            {
                cart = JsonConvert.DeserializeObject<Cart>(key);
            }
            Order order = new Order
            {
                OrderID = DateTime.Now.ToString(),
                Id = user.Id,
                CreateDate = DateTime.Now,
                TotalPrice = totalPrice,
            };
            db.Orders.Add(order);
            foreach (KeyValuePair<string, OrderDetail> item in cart.CartProduct)
            {
                count++;
                string productID = item.Value.Product.ProductID;
                Product pro = db.Products.Find(productID);
                int quan = item.Value.Quantity;
                if (pro.Quantity - item.Value.Quantity < 0)
                {
                    result = false;
                }
                pro.Quantity = pro.Quantity - item.Value.Quantity;
                totalPrice += item.Value.Quantity * pro.Price;
                db.Products.Update(pro);
                OrderDetail orderDetail = new OrderDetail
                {
                    OrderDetailID = DateTime.Now.ToString() + count,
                    ProductID = productID,
                    Quantity = item.Value.Quantity,
                    CreatedDate = DateTime.Now,
                    OrderID = order.OrderID,
                };
                db.OrderDetails.Add(orderDetail);
            }
            if (result)
            {
                db.SaveChanges();
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
