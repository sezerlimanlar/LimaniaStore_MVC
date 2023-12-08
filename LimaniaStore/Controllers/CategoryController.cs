using LimaniaStore.Data;
using LimaniaStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

	}
}
