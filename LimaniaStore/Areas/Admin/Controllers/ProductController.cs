using Limania.DataAccess.Repository.IRepository;
using Limania.Models;
using Limania.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace LimaniaStore.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ProductController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		public ProductController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<IActionResult> Index()
		{
			IEnumerable<Product> product = _unitOfWork.Product.GetAll();
			return View(product);
		}
		public async Task<IActionResult> Upsert(int? id)
		{
			ProductVM productVM = new ProductVM()
			{
				CategoryList = _unitOfWork.Category.GetAll().Select(c => new SelectListItem
				{
					Text = c.Name,
					Value = c.Id.ToString()
				}),
				Product = new Product()
			};
			if (id == null || id == 0)
			{
				//create
				return View(productVM);
			}
			else
			{
				//update
				productVM.Product = await _unitOfWork.Product.Get(p => p.Id == id);
				return View(productVM);
			}
		}
		[HttpPost]
		public async Task<IActionResult> Upsert(ProductVM productVM, IFormFile? file)
		{
			if (ModelState.IsValid)
			{
				_unitOfWork.Product.Add(productVM.Product);
				TempData["success"] = "Product Ekleme Başarılı!";
				await _unitOfWork.Save();
				return RedirectToAction("Index", "Product");
			}
			else
			{
				productVM.CategoryList = _unitOfWork.Category.GetAll().Select(c => new SelectListItem
				{
					Text = c.Name,
					Value = c.Id.ToString()
				});
				return View(productVM);
			}
		}

		public async Task<IActionResult> Delete(int? id)
		{
			Product product = await _unitOfWork.Product.Get(p => p.Id == id);
			if (product == null)
			{
				return NotFound();
			}
			_unitOfWork.Product.Remove(product);
			TempData["success"] = "Product Silme Başarılı!";
			await _unitOfWork.Save();
			return RedirectToAction("Index", "Product");

		}
	}
}
