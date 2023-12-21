using Limania.DataAccess.Migrations;
using Limania.DataAccess.Repository.IRepository;
using Limania.Models;
using Limania.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace LimaniaStore.Areas.Customer.Controllers
{
	[Area("Customer")]
	public class HomeController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
		{
			_logger = logger;
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

			if (claim != null)
			{
				var count = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value).Count();
				HttpContext.Session.SetInt32(SD.SessionCart, count);
			}

			IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category,ProductImages");
			return View(productList);
		}

		public async Task<IActionResult> Details(int productId)
		{
			ShoppingCart cart = new()
			{
				Product = await _unitOfWork.Product.Get(p => p.Id == productId, includeProperties: "Category,ProductImages"),
				Count = 1,
				ProductId = productId,
			};
			return View(cart);
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Details(ShoppingCart shoppingCart)
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
			shoppingCart.ApplicationUserId = userId;

			ShoppingCart cartFromDb = await _unitOfWork.ShoppingCart.Get(u => u.ApplicationUserId == userId && u.ProductId == shoppingCart.ProductId);

			if (cartFromDb != null)
			{
				cartFromDb.Count += shoppingCart.Count;
				_unitOfWork.ShoppingCart.Update(cartFromDb);
			}
			else
			{
				_unitOfWork.ShoppingCart.Add(shoppingCart);
				await _unitOfWork.Save();
				var count = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId).Count();
				HttpContext.Session.SetInt32(SD.SessionCart, count);

			}
			TempData["success"] = "Cart updated succesfully";

			return RedirectToAction("Index");
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
