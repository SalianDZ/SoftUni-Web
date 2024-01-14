using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MVCIntroDemo.ViewModels.Product;
using Newtonsoft.Json;
using System.Text;
using static MVCIntroDemo.Seeding.ProductData;

namespace MVCIntroDemo.Controllers
{
	public class ProductController : Controller
	{
		public IActionResult All(string keyword = "")
		{
			if (string.IsNullOrWhiteSpace(keyword))
			{
				return View(Products);
			}

			IEnumerable<ProductViewModel> productsAfterSearch = Products
				.Where(p => p.Name.ToLower().Contains(keyword.ToLower()))
				.ToArray();
			return View(productsAfterSearch);
		}

		public IActionResult ById(string id)
		{
			ProductViewModel? product = Products
				.FirstOrDefault(p => p.Id.ToString().Equals(id));

			if (product == null)
			{
				return RedirectToAction("All");
			}

			return View(product);
		}

		public ActionResult AllAsJson() 
		{
			string jsonText = JsonConvert.SerializeObject(Products, Formatting.Indented);
			return Json(jsonText);
		}

		public ActionResult DownloadProductsInfo()
		{
			StringBuilder sb = new();

			foreach (var product in Products)
			{
				sb
					.AppendLine($"Product with Id: {product.Id}")
					.AppendLine($"## Product Name: {product.Name}")
					.AppendLine($"## Price Price: {product.Price:f2}")
					.AppendLine("-------------------------------------");
			}

			Response.Headers.Add(HeaderNames.ContentDisposition, "attachment;filename=products.txt");

			return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/plain");
		}
	}
}
