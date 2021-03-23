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
    
    public class HomeController : Controller
    {

        private ApplicationDbContext db;
        private Cart cart;
        public HomeController(ApplicationDbContext db)
        {
            this.db = db;
        }

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
            //var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            //var userID = user.Id;
            var key = HttpContext.Session.GetString("CART");
            var product = db.Products.Find(id);
            var orderDetail = new OrderDetail
            {
                Product = product,
                Quantity = 1,
            };
            if(key == null)
            {
                cart = new Cart();
            }
            else
            {
                cart = JsonConvert.DeserializeObject<Cart>(key);
            }
            cart.add(orderDetail);
            HttpContext.Session.SetString("CART", JsonConvert.SerializeObject(cart));
            return View(cart);
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
            return View(cart);            
        }

        public IActionResult ShoppingDelete(string id)
        {
            var key = HttpContext.Session.GetString("CART");
            cart = JsonConvert.DeserializeObject<Cart>(key);
            cart.delete(id);
            HttpContext.Session.SetString("CART", JsonConvert.SerializeObject(cart));
            return View(cart);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
