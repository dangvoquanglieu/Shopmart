using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
            var key = HttpContext.Session.GetString("CART");
            var product = db.Products.Find(id);
            var orderDetail = new OrderDetail
            {
                Product = product,
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
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
