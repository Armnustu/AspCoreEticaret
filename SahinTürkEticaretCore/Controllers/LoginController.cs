using Microsoft.AspNetCore.Mvc;
using SahinTürkEticaretCore.Models;
using System.Text;
using XSystem.Security.Cryptography;

namespace SahinTürkEticaretCore.Controllers
{
	public class LoginController : Controller
	{
		[HttpGet]
		public IActionResult Index()
		{
			return View();
		}
		[ValidateAntiForgeryToken]//Md5 paketi için Xact.core.pcl nugetten yükledik
		[HttpPost]
		public async Task<IActionResult> Index([Bind("Email,Password,NameSurname")]User user)
		{
			Cls_User cls_User = new Cls_User();
			if (ModelState.IsValid)
			{
				bool durum = await cls_User.Login(user);
				if (durum==false)
				{
					ViewBag.data = "şifre yada password yanlış";
				}
				if (durum == true)
				{
					return RedirectToAction("Index", "Admin");
				}
			}
			else
			{
				ViewBag.data = "Bütün Alanları kontrol ediniz";
			}
			return View();	
		}
	




	}
}
