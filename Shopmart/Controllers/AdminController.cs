using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shopmart.Data;
using Shopmart.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Shopmart.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        private ApplicationDbContext db;
        private readonly IWebHostEnvironment environment;
        public AdminController(ApplicationDbContext db, IWebHostEnvironment environment)
        {
            this.db = db;
            this.environment = environment;
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

        public IActionResult Create()
        {
            CategoryDropDownList();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                string UrlImage = "";
                var files = HttpContext.Request.Form.Files;
                foreach (var Image in files)
                {
                    if (Image != null && Image.Length > 0)
                    {
                        var file = Image;

                        var uploads = Path.Combine(environment.WebRootPath, "images");
                        if (file.Length > 0)
                        {
                            var fileName = Guid.NewGuid().ToString().Replace("-", "") + file.FileName;
                            using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                                UrlImage = fileName;
                            }

                        }
                    }
                }
                var data = new Product()
                {
                    ProductID = product.ProductID,
                    ProductName = product.ProductName,
                    UrlImage = UrlImage,
                    Price = product.Price,
                    Description = product.Description,
                    Quantity = product.Quantity,
                    CategoryID = product.CategoryID
                };
                db.Products.Add(data);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            CategoryDropDownList();
            return View();
        }


        private void CategoryDropDownList(object categorySelect = null)
        {
            var rolesQuery = db.Categories.ToList();
            ViewBag.Categories = new SelectList(rolesQuery, "CategoryID", "CategoryName", categorySelect);
        }

        public IActionResult Edit(string id)
        {
            var product = db.Products.Find(id);
            CategoryDropDownList();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, Product product)
        {
            if (ModelState.IsValid)
            {
                string UrlImage = "";
                var files = HttpContext.Request.Form.Files;
                foreach (var Image in files)
                {
                    if (Image != null && Image.Length > 0)
                    {
                        var file = Image;

                        var uploads = Path.Combine(environment.WebRootPath, "images");
                        if (file.Length > 0)
                        {
                            var fileName = Guid.NewGuid().ToString().Replace("-", "") + file.FileName;
                            using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                                UrlImage = fileName;
                            }

                        }
                    }
                }
                var data = db.Products.Find(id);
                data.ProductName = product.ProductName;
                data.UrlImage = UrlImage;
                data.Price = product.Price;
                data.Description = product.Description;
                data.Quantity = product.Quantity;
                data.CategoryID = product.CategoryID;
                db.Products.Update(data);
                await db.SaveChangesAsync();
                //  return RedirectToAction(nameof(Edit), new { id = data.idProduct });
                return RedirectToAction(nameof(Index));
            }
            CategoryDropDownList();
            return View();
        }

        public async Task<ActionResult> Delete(string id)
        {
            var data = db.Products.Find(id);
            db.Products.Remove(data);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
