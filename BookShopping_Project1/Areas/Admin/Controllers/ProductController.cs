using BookShopping_Project.DataAccess.Repository.IRepository;
using BookShopping_Project.Models;
using BookShopping_Project.Models.ViewModels;
using BookShopping_Project.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopping_Project1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class ProductController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IWebHostEnvironment _webHostEnviroment;
        public ProductController(IUnitOfWork unitOfWork,IWebHostEnvironment webHostEnvironment)
        {
            _UnitOfWork = unitOfWork;
            _webHostEnviroment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategoryList = _UnitOfWork.Category.GetAll().Select(cl => new SelectListItem()
                {
                    Text = cl.Name,
                    Value = cl.Id.ToString()
                }),
                CoverTypeList=_UnitOfWork.CoverType.GetAll().Select(ct=>new SelectListItem()
                {
                    Text=ct.Name,
                    Value=ct.Id.ToString()
                })
                
            };
            if (id == null)
                return View(productVM);
            //productVM.Product = _UnitOfWork.Product.Get(id.GetValueOrDefault());
           productVM.Product = _UnitOfWork.Product.FirstOrDefault(P => P.Id == id.GetValueOrDefault(), includeproperties: "Category,CoverType");
            return View(productVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid==false)
            {
                var webrootpath = _webHostEnviroment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    var filename = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webrootpath, @"Images\Products");
                    var extension = Path.GetExtension(files[0].FileName);
                    if (productVM.Product.Id != 0)
                    {
                        var imageExists = _UnitOfWork.Product.Get(productVM.Product.Id).ImageUrl;
                        productVM.Product.ImageUrl = imageExists;
                    }
                    if (productVM.Product.ImageUrl != null)
                    {
                        var imagePath = Path.Combine
                            (webrootpath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(uploads, filename + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    productVM.Product.ImageUrl = @"\Images\Products\" + filename + extension;

                }
                else
                {
                    if (productVM.Product.Id != 0)
                    {
                        var imageExists = _UnitOfWork.Product.Get(productVM.Product.Id).ImageUrl;
                        productVM.Product.ImageUrl = imageExists;
                    }
                }
                if (productVM.Product.Id == 0)
                    _UnitOfWork.Product.Add(productVM.Product);
                else
                    _UnitOfWork.Product.Update(productVM.Product);
                _UnitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                productVM = new ProductVM()
                {
                    CategoryList = _UnitOfWork.Category.GetAll().
                    Select(cl => new SelectListItem()
                    {
                        Text = cl.Name,
                        Value = cl.Id.ToString()
                    }),
                    CoverTypeList = _UnitOfWork.CoverType.GetAll().
                    Select(ct => new SelectListItem()
                    {
                        Text = ct.Name,
                        Value = ct.Id.ToString()
                    })
                };
                if (productVM.Product.Id != 0)
                {
                    productVM.Product = _UnitOfWork.Product.Get
                        (productVM.Product.Id);
                }
                return View(productVM);
            }

        }
        #region API's
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _UnitOfWork.Product.GetAll(includeproperties: "Category,CoverType");
            return Json(new { data = productList });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var productInDb = _UnitOfWork.Product.Get(id);
            if (productInDb == null)
                return Json(new { success = false, message = "Error while delete data !!" });
            if (productInDb.ImageUrl != "")
            {
                var webrootPath = _webHostEnviroment.WebRootPath;
                var imagepath = Path.Combine(webrootPath, productInDb.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(imagepath))
                {
                    System.IO.File.Delete(imagepath);
                }
            }
            _UnitOfWork.Product.Remove(productInDb);
            _UnitOfWork.Save();
            return Json(new { success = true, message = "Date delete successfully !!!" });
        }
        #endregion
    }
}
