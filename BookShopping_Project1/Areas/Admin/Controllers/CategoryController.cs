using BookShopping_Project.DataAccess.Repository.IRepository;
using BookShopping_Project.Models;
using BookShopping_Project.Models.ViewModels;
using BookShopping_Project.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopping_Project1.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitofWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitofWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            Category category = new Category();
            var model = new pcvm
            {
                Category=new Category(),
                Attributesproducts = Getattribute()
            };
            if (id == null)
                return View(model);
            else
            {
                List<string> get=new List<string>();
                var innerJoin = (from cat in _unitofWork.Category.GetAll()
                                join pc in _unitofWork.pc.GetAll() on cat.Id equals pc.CategoryId
                                select new{
                                    pc.Name
                }).ToList().AsQueryable();
                var mode = new pcvm
                {
                    Attributesproducts = Getattribute(),
                    Category = _unitofWork.Category.Get(id.GetValueOrDefault()),
                SelectedFruits = _unitofWork.pc.GetAll(u => u.CategoryId == id).Select(u => u.Name).ToList()
                };
                return View(mode);

            }
        }

        [HttpPost]
        public IActionResult Upsert(pcvm pcc)
        { 
            var duplicate=_unitofWork.Category.GetAll(x=>x.Id!=pcc.Category.Id && x.Name==pcc.Category.Name).Count();

            if (duplicate==0)
            {
                
                if (pcc == null)
                    return NotFound();
                if (!ModelState.IsValid)
                    return View(pcc);
                if (pcc.Category.Id == 0)
                {

                    _unitofWork.Category.Add(pcc.Category);
                    _unitofWork.Save();;
                    for (int i = 0; i < pcc.SelectedFruits.Count(); i++)
                    {
                        pc obj = new pc()
                        {
                            Name = pcc.SelectedFruits[i],
                            CategoryId=pcc.Category.Id
                           
                        };
                        _unitofWork.pc.Add(obj);
                        _unitofWork.Save();
                    }
                }
                else
                {
                    _unitofWork.Category.Update(pcc.Category);
                    _unitofWork.Save();
                    for (int i = 0; i < pcc.SelectedFruits.Count(); i++)
                    {
                        pc obj = new pc()
                        {
                            Name = pcc.SelectedFruits[i],
                            CategoryId = pcc.Category.Id

                        };
                        _unitofWork.pc.Update(obj);
                        _unitofWork.Save();
                    }

                }
            }

            else
            {
                 TempData["msg"] = "its duplicate here";
                return RedirectToAction("Upsert");
               // return View(category);

            }
            return RedirectToAction(nameof(Index));
        }
        private IList<SelectListItem> Getattribute()
        {
            return new List<SelectListItem>
        {
            new SelectListItem {Text = "Apple", Value = "Apple"},
            new SelectListItem {Text = "Pear", Value = "Pear"},
            new SelectListItem {Text = "Banana", Value = "Banana"},
            new SelectListItem {Text = "Orange", Value = "Orange"},
        };
        }
        #region API's
        [HttpGet]
        public IActionResult GetAll()
        {
            var catagoryList = _unitofWork.Category.GetAll();
            return Json(new { data = catagoryList });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var categoryInDb = _unitofWork.Category.Get(id);
            if (categoryInDb == null)
                return Json(new { success = false, message = "Error while delete data !!" });
            _unitofWork.Category.Remove(categoryInDb);
            _unitofWork.Save();
            return Json(new { success = true, message = "Data delete Successfully !!!" });


        }
        #endregion
    }
}
