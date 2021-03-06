﻿using MvcByAliAzavi.Data;
using MvcByAliAzavi.Models;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace MvcByAliAzavi.Controllers
{
  public class Products2Controller : Controller
  {
    private LearningContext db = new LearningContext();


    // GET: Products2
    public ActionResult Index()
    {
      var model = db.Products2.ToList();
      return View(model);
    }

    // GET: Products/Create
    public ActionResult Create()
    {
      // To Bind the category drop down in search section
      ViewBag.Categories = db.Categories.Where(x => x.IsActive == true);
      var model = new ProductModel2();
      return View(model);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(ProductModel2 model)
    {
      var imageTypes = new string[]{
                    "image/gif",
                    "image/jpeg",
                    "image/pjpeg",
                    "image/png"
                };
      if (model.ImageUpload == null || model.ImageUpload.ContentLength == 0)
      {
        ModelState.AddModelError("ImageUpload", "This field is required");
      }
      else if (!imageTypes.Contains(model.ImageUpload.ContentType))
      {
        ModelState.AddModelError("ImageUpload", "Please choose either a GIF, JPG or PNG image.");
      }

      if (ModelState.IsValid)
      {
        var product = new Product2();
        product.ProductName = model.ProductName;
        product.Price = model.Price;
        product.CategoryId = model.CategoryId;
        using (var binaryReader = new BinaryReader(model.ImageUpload.InputStream))
          product.Image = binaryReader.ReadBytes(model.ImageUpload.ContentLength);

        db.Products2.Add(product);
        db.SaveChanges();
        return RedirectToAction("Index");
      }

      // If any error return back to the page
      ViewBag.Categories = db.Categories.Where(x => x.IsActive == true);
      return View(model);
    }


  }
}