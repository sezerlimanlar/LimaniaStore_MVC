using LimaniaStore.Data;
using LimaniaStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LimaniaStore.Controllers
{
	public class CategoryController : Controller
	{
		private readonly ApplicationDbContext _db;
		public CategoryController(ApplicationDbContext db)
		{
			_db = db;
		}

		public async Task<IActionResult> Index()
		{
			List<Category> datas = await _db.Categories.ToListAsync();
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

				await _db.Categories.AddAsync(category);
				TempData["success"] = "Kategori Ekleme Başarılı!";
				await _db.SaveChangesAsync();
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
			Category? category = await _db.Categories.FindAsync(id);
			if (category == null)
			{
				return NotFound();
			}
			return View(category);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(Category category)
		{
			_db.Categories.Update(category);
			TempData["success"] = "Kategori Düzenleme Başarılı!";
			_db.SaveChanges();
			return RedirectToAction("Index", "Category");
		}

		public async Task<IActionResult> Delete(int? id)
		{
			Category? obj = await _db.Categories.FindAsync(id);
			if (obj == null)
			{
				return NotFound();
			}
			_db.Categories.Remove(obj);
			TempData["success"] = "Kategori Silme Başarılı!";
			await _db.SaveChangesAsync();
			return RedirectToAction("Index", "Category");
		}
	}
}
