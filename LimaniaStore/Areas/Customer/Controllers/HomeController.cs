using Limania.DataAccess.Repository.IRepository;
using Limania.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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
			IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category");
			return View(productList);
		}

		public async Task<IActionResult> Details(int productId)
		{
			Product product = await _unitOfWork.Product.Get(p => p.Id == productId, includeProperties: "Category");
			return View(product);
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
