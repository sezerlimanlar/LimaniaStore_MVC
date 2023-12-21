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
	[Authorize(Roles = SD.Role_Admin)]

	public class ProductController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IWebHostEnvironment _webHostEnvironment;
		public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
		{
			_unitOfWork = unitOfWork;
			_webHostEnvironment = webHostEnvironment;
		}

		public IActionResult Index()
		{
			List<Product> product = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
			return View(product);
		}
		public async Task<IActionResult> Upsert(int? id)
		{
			ProductVM productVM = new()
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
				productVM.Product = await _unitOfWork.Product.Get(p => p.Id == id, includeProperties: "ProductImages");
				return View(productVM);
			}
		}



		[HttpPost]
		public async Task<IActionResult> Upsert(ProductVM productVM, List<IFormFile> files)
		{
			if (ModelState.IsValid)
			{
				if (productVM.Product.Id == 0)
				{
					_unitOfWork.Product.Add(productVM.Product);
					TempData["success"] = "Product Güncelleme Başarılı!";

				}
				else
				{
					_unitOfWork.Product.Update(productVM.Product);
					TempData["success"] = "Product Ekleme Başarılı!";
				}

				await _unitOfWork.Save();

				string wwwRootPath = _webHostEnvironment.WebRootPath;
				if (files != null)
				{

					foreach (IFormFile file in files)
					{
						string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
						string productPath = @"images\products\product-" + productVM.Product.Id;
						string finalPath = Path.Combine(wwwRootPath, productPath);

						if (!Directory.Exists(finalPath))
							Directory.CreateDirectory(finalPath);


						using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
						{
							file.CopyTo(fileStream);
						}

						ProductImage productImage = new()
						{
							ImageUrl = @"\" + productPath + @"\" + fileName,
							ProductId = productVM.Product.Id,
						};

						if (productVM.Product.ProductImages == null)
							productVM.Product.ProductImages = new List<ProductImage>();

						productVM.Product.ProductImages.Add(productImage);

					}

					_unitOfWork.Product.Update(productVM.Product);
					await _unitOfWork.Save();
				}
				TempData["success"] = "Product Created/Updated Successfully!";
				return RedirectToAction("Index");
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
		public async Task<IActionResult> DeleteImage(int imageId)
		{
			var imageToBeDeleted = await _unitOfWork.ProductImage.Get(u => u.Id == imageId);
			int productId = imageToBeDeleted.ProductId;
			if (imageToBeDeleted != null)
			{
				if (!string.IsNullOrEmpty(imageToBeDeleted.ImageUrl))
				{
					var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, imageToBeDeleted.ImageUrl.TrimStart('\\'));

					if (System.IO.File.Exists(oldImagePath))
					{
						System.IO.File.Delete(oldImagePath);
					}
				}
				_unitOfWork.ProductImage.Remove(imageToBeDeleted);
				await _unitOfWork.Save();

				TempData["success"] = "Delete Successfully!";

			}
			return RedirectToAction(nameof(Upsert), new {id= productId });
		}

		#region API CALLS
		public IActionResult GetAll()
		{
			List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
			return Json(new { data = objProductList });
		}

		[HttpDelete]
		public async Task<IActionResult> Delete(int? id)
		{
			var productToBeDeleted = await _unitOfWork.Product.Get(p => p.Id == id);
			if (productToBeDeleted == null)
			{
				return Json(new { success = false, message = "Error while deleting" });
			}

			string productPath = @"images\products\product-" + id;
			string finalPath = Path.Combine(_webHostEnvironment.WebRootPath, productPath);

			if (!Directory.Exists(finalPath))
			{
				string[] filePaths = Directory.GetFiles(finalPath);
				foreach(var filePath in filePaths)
				{
					System.IO.File.Delete(filePath);	
				}
				Directory.Delete(finalPath);
			}

			_unitOfWork.Product.Remove(productToBeDeleted);
			await _unitOfWork.Save();
			return Json(new { success = false, message = "Delete successful" });

		}
		#endregion
	}
}
