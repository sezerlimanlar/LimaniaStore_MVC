using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Limania.DataAccess.Data;
using Limania.Models;
using Limania.DataAccess.Repository.IRepository;
namespace LimaniaStore.Controllers
{
	public class CategoryController : Controller
	{
		private readonly ICategoryRepository _categoryRepository;
		public CategoryController(ICategoryRepository db)
		{
			_categoryRepository = db;
		}

		public async Task<IActionResult> Index()
		{
			List<Category> datas = _categoryRepository.GetAll().ToList();
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

				_categoryRepository.Add(category);
				TempData["success"] = "Kategori Ekleme Başarılı!";
				_categoryRepository.Save();
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
			Category? category = _categoryRepository.Get(c => c.Id == id);
			if (category == null)
			{
				return NotFound();
			}
			return View(category);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(Category category)
		{
			_categoryRepository.Update(category);
			TempData["success"] = "Kategori Düzenleme Başarılı!";
			_categoryRepository.Save();
			return RedirectToAction("Index", "Category");
		}

		public async Task<IActionResult> Delete(int? id)
		{
			Category? obj = _categoryRepository.Get(c => c.Id == id);
			if (obj == null)
			{
				return NotFound();
			}
			_categoryRepository.Remove(obj);
			TempData["success"] = "Kategori Silme Başarılı!";
			_categoryRepository.Save();
			return RedirectToAction("Index", "Category");
		}
	}
}
