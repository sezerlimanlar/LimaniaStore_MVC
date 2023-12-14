using Limania.DataAccess.Repository.IRepository;
using Limania.Models;
using Limania.Models.ViewModels;
using Limania.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace LimaniaStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    /*	[Authorize(Roles = SD.Role_Admin)]
    */
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Company> company = _unitOfWork.Company.GetAll();
            return View(company);
        }
        public async Task<IActionResult> Upsert(int? id)
        {

            if (id == null || id == 0)
            {
                //create
                return View(new Company());
            }
            else
            {
                //update
                Company company = await _unitOfWork.Company.Get(p => p.Id == id);
                return View(company);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Upsert(Company company)
        {
            if (ModelState.IsValid)
            {

                if (company.Id == 0)
                {
                    _unitOfWork.Company.Add(company);
                    TempData["success"] = "Company Güncelleme Başarılı!";

                }
                else
                {
                    _unitOfWork.Company.Update(company);
                    TempData["success"] = "Company Ekleme Başarılı!";
                }
                await _unitOfWork.Save();
                return RedirectToAction("Index", "Company");
            }
            else
            {
                return View(company);
            }

        }




        //GET İSTEĞİ DÜZ VERİ SİLME İŞLEMİ
        /*public async Task<IActionResult> Delete(int? id)
		{
			Company product = await _unitOfWork.Company.Get(p => p.Id == id);
			if (product == null)
			{
				return NotFound();
			}
			_unitOfWork.Company.Remove(product);
			TempData["success"] = "Company Silme Başarılı!";
			await _unitOfWork.Save();
			return RedirectToAction("Index", "Company");

		}*/





        #region API CALLS
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = objCompanyList });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            var productToBeDeleted = await _unitOfWork.Company.Get(p => p.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Company.Remove(productToBeDeleted);
            await _unitOfWork.Save();
            return Json(new { success = false, message = "Delete successful" });

        }
        #endregion
    }
}
