using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        public HomeController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            var listProduct = db.Products.Select(s => new Product
            {
                ProductID = s.ProductID,
                ProductName = s.ProductName,
                UrlImage = s.UrlImage,
                Price = s.Price,
                Description = s.Description,
                Quantity = s.Quantity,
                Category = db.Categories.Where(c => c.CategoryID == s.CategoryID).FirstOrDefault()
            }).ToList();
            return View(listProduct);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Roles = "User")]
        public IActionResult Shopping()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
