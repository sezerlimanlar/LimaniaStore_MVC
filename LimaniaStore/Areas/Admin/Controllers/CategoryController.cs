using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Limania.DataAccess.Data;
using Limania.Models;
using Limania.DataAccess.Repository.IRepository;
namespace LimaniaStore.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class CategoryController : Controller
	{

		private readonly IUnitOfWork _unitOfWork;
		public CategoryController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<IActionResult> Index()
		{
			IEnumerable<Category> datas = _unitOfWork.Category.GetAll();
			return View(datas);

		}
		public async Task<IActionResult> Create()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Create(Category category)
		{
			/*  if(category.Name == category.DisplayOrder.ToString())
			  {
				  // İlk parametre hangi input altında error mesajının gözükmesini istiyorsak onu belirtir. (Custom error message)
				  ModelState.AddModelError("", "Kategori ismi ve kategori sıralaması aynı isim girilemez."); 
			  }*/
			if (ModelState.IsValid)
			{

				_unitOfWork.Category.Add(category);
				TempData["success"] = "Kategori Ekleme Başarılı!";
				await _unitOfWork.Save();
				return RedirectToAction("Index", "Category");
			}
			return View();
		}
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			Category? category = await _unitOfWork.Category.Get(c => c.Id == id);
			if (category == null)
			{
				return NotFound();
			}
			return View(category);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(Category category)
		{
			_unitOfWork.Category.Update(category);
			TempData["success"] = "Kategori Düzenleme Başarılı!";
			_unitOfWork.Save();
			return RedirectToAction("Index", "Category");
		}

		public async Task<IActionResult> Delete(int? id)
		{
			Category? obj = await _unitOfWork.Category.Get(c => c.Id == id);
			if (obj == null)
			{
				return NotFound();
			}
			_unitOfWork.Category.Remove(obj);
			TempData["success"] = "Kategori Silme Başarılı!";
			await _unitOfWork.Save();
			return RedirectToAction("Index", "Category");
		}
	}
}
