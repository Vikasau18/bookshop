using BookShopping_Project.DataAccess.Repository.IRepository;
using BookShopping_Project.Models;
using BookShopping_Project.Utility;
using BookShopping_Project1.Models;
using BookShopping_Project1.Models.ViewModels;
using eshop.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookShopping_Project1.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger,IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;

        }

        public IActionResult Index(Myproductvm myproductvm)
        {
            Myproductvm productVM = new Myproductvm()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(c1 => new SelectListItem()
                {
                    Text = c1.Name,
                    Value = c1.Id.ToString()
                })
            };
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                var count = _unitOfWork.ShoppingCart.GetAll(sc => sc.ApplicationUserId == claim.Value).ToList().Count;
                HttpContext.Session.SetInt32(SD.Ss_session, count);
            }
            //var productList = _unitOfWork.Product.GetAll(includeproperties: "Category,CoverType");
            //return View(productList);
            if (myproductvm.Product == null)
            {
                var productList = _unitOfWork.Product.GetAll(includeproperties: "Category,CoverType");
                ViewData["prod"] = productList;
                productVM.ProductList = productList;
                return View(productVM);

            }
            else
            {
                var productList = _unitOfWork.Product.GetAll(x => x.CategoryId == myproductvm.Product.CategoryId, includeproperties: "Category,CoverType");
                productVM.ProductList = productList;
                return View(productVM);
            }
        }

        public IActionResult Details(int id)
        {
            var productInDb = _unitOfWork.Product.FirstOrDefault(p => p.Id == id, includeproperties: "Category,CoverType");
            var shoppingCart = new ShoppingCart()
            {
                Product=productInDb,
                ProductId=productInDb.Id
            };
            return View(shoppingCart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCartObj)
        {
            shoppingCartObj.Id = 0;
            if (ModelState.IsValid)
            {
                //accessing login user
                var claimsIdentity = (ClaimsIdentity)User.Identity;

                //to access specific user identity(name)
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                shoppingCartObj.ApplicationUserId = claim.Value;

                //check shopping cart

                var shoppingCartFromDb = _unitOfWork.ShoppingCart.FirstOrDefault(u => u.ApplicationUserId == claim.Value && u.ProductId == shoppingCartObj.ProductId);
                if (shoppingCartFromDb == null)
                    _unitOfWork.ShoppingCart.Add(shoppingCartObj);

                else
                    shoppingCartFromDb.Count += shoppingCartFromDb.Count;
                _unitOfWork.Save();
                //session
                var count = _unitOfWork.ShoppingCart.GetAll(sc => sc.ApplicationUserId == claim.Value).ToList().Count;
                //adding count in session

                HttpContext.Session.SetInt32(SD.Ss_session, count);
                return RedirectToAction(nameof(Index));


            }

            else {
                var productInDb = _unitOfWork.Product.FirstOrDefault(p => p.Id == shoppingCartObj.ProductId, includeproperties: "Category, CoverType");
                var shoppingCart = new ShoppingCart()
                {
                    Product = productInDb,
                    ProductId = productInDb.Id
                };
                return View(shoppingCart);
            }
        }
        public IActionResult Privacy()
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
