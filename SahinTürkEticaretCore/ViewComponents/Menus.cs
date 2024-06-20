using Microsoft.AspNetCore.Mvc;
using SahinTürkEticaretCore.Models;

namespace SahinTürkEticaretCore.ViewComponents
{
	public class Menus:ViewComponent
	{
		DataContext context = new DataContext();
		public IViewComponentResult Invoke()
		{
			List<Category> categories = context.categories.Where(s=>s.Active==true).ToList();
			return View(categories);
		}

	}
}
