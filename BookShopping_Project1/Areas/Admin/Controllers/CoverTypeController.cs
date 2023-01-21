using BookShopping_Project.DataAccess.Repository.IRepository;
using BookShopping_Project.Models;
using BookShopping_Project.Utility;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopping_Project1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            CoverType coverType = new CoverType();
            if (id == null)

                return View(coverType);
            var param = new DynamicParameters();
            param.Add("@Id", id.GetValueOrDefault());
            coverType=_unitOfWork.SP_CALL.OneRecord<CoverType>(SD.Proc_CoverType_GetCoverType,param);
           // coverType = _unitOfWork.CoverType.Get(id.GetValueOrDefault());
            if (coverType == null)
                return NotFound();
            return View(coverType);
        }
        [HttpPost]
        public IActionResult Upsert (CoverType coverType)
        {
            if (coverType == null)
                return NotFound();
            if (!ModelState.IsValid)
                return View(coverType);
            var param = new DynamicParameters();
            param.Add("@Name", coverType.Name);
            if (coverType.Id == 0)
                _unitOfWork.SP_CALL.Execute(SD.Proc_CoverType_Create, param);
            //if (coverType.Id == 0)
            // _unitOfWork.CoverType.Add(coverType);
            else
            {
                param.Add("@Id", coverType.Id);
                _unitOfWork.SP_CALL.Execute(SD.Proc_CoverType_Update, param);
            }
                //_unitOfWork.CoverType.Update(coverType);
           // _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        #region API's
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.SP_CALL.List<CoverType>(SD.Proc_CoverType_GetCoverTypes)});
           // return Json(new { data = _unitOfWork.CoverType.GetAll()});
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var coverTypeInDb = _unitOfWork.CoverType.Get(id);
            if (coverTypeInDb == null)
                return Json(new { success = false, message = "Error While Delete Data !!" });
            var param = new DynamicParameters();
            param.Add("@Id", id);
            _unitOfWork.SP_CALL.Execute(SD.Proc_CoverType_Delete, param);
           // _unitOfWork.CoverType.Remove(coverTypeInDb);
            //_unitOfWork.Save();
            return Json(new { success = true, message = "Delete Data Successfully !!!" });
        }
        #endregion
    }
}
