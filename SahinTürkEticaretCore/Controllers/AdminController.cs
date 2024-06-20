using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Microsoft.Identity.Client.Kerberos;
using SahinTürkEticaretCore.Migrations;
using SahinTürkEticaretCore.Models;
using System.Drawing.Design;
using System.Net;
using System.Runtime.CompilerServices;
//using XAct;

namespace SahinTürkEticaretCore.Controllers
{
	public class AdminController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult Giris()
		{
			return View();
		}
		[ValidateAntiForgeryToken]//Md5 paketi için Xact.core.pcl nugetten yükledik
		[HttpPost]
		public async Task<IActionResult> Giris([Bind("Email,Password,NameSurname")] User user)
		{
			Cls_User cls_User = new Cls_User();
			if (ModelState.IsValid)
			{
				bool durum = await cls_User.Login(user);
				if (durum == false)
				{
					ViewBag.data = "şifre yada password yanlış";
				}
				if (durum == true)
				{
					return RedirectToAction("Index", "Home");
				}
			}
			else
			{
				ViewBag.data = "Bütün Alanları kontrol ediniz";
			}
			return View();
		}
		public IActionResult CategoryIndex()
		{

			List<Category> categories = cls_Category.CategorySelect("all");
			return View(categories);
		}
		Cls_User cls_User = new Cls_User();
		Cls_Category cls_Category = new Cls_Category();
		//void categoryFill()
		//{
		//	List<Category> categories = cls_Category.CategorySelect("all");
		//	ViewData["categoryList"] = categories.Select(c => new SelectListItem { Text = c.CategoryName, Value = c.CategoryID.ToString() });
		//}
		public async Task<IActionResult> CategoryCreate()
		{
			CategoryFill("main");
			return View();
		}

		[HttpPost]
		public IActionResult CategoryCreate([Bind("ParentID,CategoryName,Active")] Category category)
		{
			if (ModelState.IsValid)
			{
				bool answer = cls_Category.CategoryInsert(category);
				if (answer == true)
				{
					TempData["Message"] = "Eklendi";
				}
				else
				{
					TempData["Message"] = "Hata";
				}
				return RedirectToAction("CategoryIndex");
			}
			else
			{
				//categoryFill();
				return View();
			}
			return RedirectToAction("CategoryCreate");

		}
		DataContext context = new DataContext();
		public async Task<IActionResult> CategoryEdit(int? id)//alt kategori sayfaya gelmiyor
		{
            CategoryFill("main");
            if (id == null || context.categories == null)
			{
				return NotFound();
			}
			var category = await cls_Category.CategoryDetails(id);
			return View(category);
		}
		[HttpPost]
		public async Task<IActionResult> CategoryEdit(Category category)
		{

		
			bool durum = cls_Category.CategoryUpdate(category);
			if (durum == true)
			{
				TempData["Message"] = "Güncellendi";
				return RedirectToAction("CategoryIndex");

			}
			else
			{

				TempData["Message"] = "Hata";
				return RedirectToAction(nameof(CategoryEdit));

			}

		}

		[HttpPost]
		public static bool CategoryUpdate(Category category)
		{
			try
			{
				using (DataContext context = new DataContext())
				{
					context.categories.Update(category);
					context.SaveChanges();
					return true;
				}

			}
			catch (Exception)
			{

				return false;
			}

		}

		public async Task<IActionResult> CategoryDetails(int? id)
		{
			var category = await cls_Category.CategoryDetails(id);
			ViewBag.Categoryname = category?.CategoryName;
			return View(category);
		}

		[HttpGet]
		public async Task<IActionResult> CategoryDelete(int? id)
		{
			if (id == null || context.categories == null)
			{
				return NotFound();

			}
			var category = await cls_Category.CategoryDetails(id);
			if (category == null)
			{
				NotFound();
			}
			return View(category);
		}

		[HttpPost, ActionName("CategoryDelete")]//Routing
		public IActionResult CategoryDeleteConfirmed(int id)
		{
			bool answer = Cls_Category.CategoryDelete(id);
			if (answer == true)
			{
				TempData["Message"] = "silindi";
				return RedirectToAction("CategoryIndex");
			}
			else
			{
				TempData["Message"] = "Hata";
				return RedirectToAction(nameof(CategoryDelete));
			}

		}

		public async Task<IActionResult> SupplierIndex()
		{
			Cls_Supplier cls_Supplier = new Cls_Supplier();

			List<Supplier> suppliers = await cls_Supplier.SupplierSelect();
			return View(suppliers);
		}
		public async Task<IActionResult> SupplierDetails(int? id)
		{
			Cls_Supplier cls_Supplier = new Cls_Supplier();
			var supplier = await cls_Supplier.supplierDetails(id);
			ViewBag.Count = context.products.Where(p => p.SupplierID == id).Count();
			ViewBag.Brandname = supplier?.BrandName;
			return View(supplier);
		}
		public async Task<IActionResult> SupplierCreate()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SupplierCreate(Supplier supplier)
		{
			Cls_Supplier cls_Supplier = new Cls_Supplier();
			if (ModelState.IsValid)
			{
				//var extension = Path.GetExtension(supplier.PhotoPath.Fi);
				//var nevigame = Guid.NewGuid() + extension;
				//var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/WriterImageFiles/", nevigame);
				//var stream = new FileStream(location, FileMode.Create);
				//addProfileImage.WriterImage.CopyTo(stream);
				//writer.WriterImage = nevigame;

				bool answer = cls_Supplier.SupplierInsert(supplier);
				if (answer == true)
				{
					TempData["Message"] = supplier.BrandName.ToUpper() + "Eklendi";
				}
				else
				{
					TempData["Message"] = "Hata";
				}
				return RedirectToAction("SupplierIndex");
			}
			else
			{

				return View();
			}
			return RedirectToAction("SupplierCreate");
		}

		public async Task<IActionResult> SupplierEdit(int? id)
		{

			Cls_Supplier cls_Supplier = new Cls_Supplier();
			if (id == null || context.suppliers == null)
			{
				return NotFound();
			}
			var supplier = await cls_Supplier.supplierDetails(id);
			return View(supplier);

		}
		[HttpPost]
		public async Task<IActionResult> SupplierEdit(Supplier supplier)
		{

			Cls_Supplier cls_Supplier = new Cls_Supplier();

			if (supplier.PhotoPath == null)
			{
				supplier.PhotoPath = context.suppliers.FirstOrDefault(x => x.SupplierID == supplier.SupplierID).PhotoPath;


			}
			bool answer = cls_Supplier.SupplierUpdate(supplier);
			if (answer == true)
			{
				TempData["Message"] = supplier.BrandName.ToUpper() + "Güncellendi";
				return RedirectToAction("SupplierIndex");

			}
			else
			{
				TempData["Message"] = "Hata";
			}
			return RedirectToAction("SupplierIndex");
		}
		[HttpGet, ActionName("Delete")]
		public async Task<IActionResult> SupplierDelete(int? id)
		{

			Cls_Supplier cls_Supplier = new Cls_Supplier();
			if (id == null || context.suppliers == null)
			{
				return NotFound();
			}

			bool durum = await cls_Supplier.SupplierDelete(id);
			if (durum == true)
			{
				var supplier = await cls_Supplier.SupplierSelect();
				return RedirectToAction("SupplierIndex");
			}
			return View();

		}
		public async Task<IActionResult> StatusIndex()
		{
			Cls_Status cls_Status = new Cls_Status();

			List<Status> statuses = await cls_Status.StatusSelect();
			return View(statuses);
		}
		public async Task<IActionResult> StatusCreate()
		{
			return View();
		}
			[HttpPost]
		public async Task<IActionResult> StatusCreate(Status status)
		{
			Cls_Status cls_Status = new Cls_Status();

			if (ModelState.IsValid)
			{
				bool answer = cls_Status.StatusInsert(status);
				if (answer == true)
				{
					TempData["Message"] = status.SatusName + "Eklendi";
					return RedirectToAction("StatusIndex");
				}

				else
				{
					TempData["Message"] = "Hata";
				}
			}
			else
			{
				View();
			}
			
			return RedirectToAction("StatusCreate");
		}
		public async Task<IActionResult> StatusEdit(int? id)
		{

			Cls_Status cls_Status = new Cls_Status();
			if (id == null || context.statuses == null)
			{
				return NotFound();
			}
			var status = await cls_Status.StatusDetails(id);
			return View(status);

		}
		[HttpPost]
		public async Task<IActionResult> StatusEdit(Status status)
		{

			Cls_Status cls_Status = new Cls_Status();
			
			
			bool answer = cls_Status.StatusUpdate(status);
			if (answer == true)
			{
				TempData["Message"] = status.SatusName.ToUpper() + "Güncellendi";
				return RedirectToAction("StatusIndex");

			}
			else
			{
				TempData["Message"] = "Hata";
			}
			return RedirectToAction("StatusIndex");
		}
		public async Task<IActionResult> StatusDetails(int? id)
		{
			Cls_Status cls_Status = new Cls_Status();
			var status = await cls_Status.StatusDetails(id);
			ViewBag.Count = context.products.Where(p => p.StatusID == id).Count();
			ViewBag.Brandname = status?.SatusName;
			return View(status);
		}
		//[HttpGet, ActionName("Delete")]
		public async Task<IActionResult> StatusDelete(int id)
		{
			
			if (id == null && context.statuses!=null)
			{
				return NotFound();
			}
			Cls_Status cls_Status = new Cls_Status();
			bool durum = await cls_Status.StatusDelete1(id);
			if (durum == true)
			{
				var status = await cls_Status.StatusSelect();
				return RedirectToAction("StatusIndex");
			}
			return View();

		}
		public async Task<IActionResult> ProductCreate()
		{
		    //buradaki metodları tanımlamamızdaki amaç burda çok kod yazmaktansa view ait bilgileri metod içinde yazmaktır kod kalabalığını engellemektir  
            SupplierFill();
            StatusFill();
            CategoryFill("all");
           				
			return View();

        }

		[HttpPost]
		public async Task<IActionResult> ProductCreate(Products product)
		{
			StatusFill();
			Cls_Product cls_Product = new Cls_Product();
			product.AddDate = DateTime.Now;
			if (ModelState.IsValid)
			{
				bool answer = await cls_Product.ProductInsert(product);
				if (answer == true)
				{
					TempData["Message"] = "Eklendi";
				}
				else
				{
					TempData["Message"] = "Hata";
				}
			}
			else
			{
				return View();
			}
			return RedirectToAction("ProductIndex");		
			
        }	
         async void SupplierFill()
		{
           
            Cls_Supplier cls_Supplier = new Cls_Supplier();
            List<Supplier> suppliers = await cls_Supplier.SupplierSelect();
            ViewBag.SupplierID = new SelectList(suppliers, "SupplierID", "BrandName");

        }
        async void CategoryFill(string main_or_all)
		{
            Cls_Category cls_Category = new Cls_Category();
            List<Category> categories = cls_Category.CategorySelect("main_or_all");
            ViewData["categoriList"] = categories.Select(c => new SelectListItem { Text = c.CategoryName, Value = c.CategoryID.ToString() });
        }
        async void StatusFill()
		{
            Cls_Status cls_Status = new Cls_Status();
            List<Status> statuses = await cls_Status.StatusSelect();
            ViewData["statusList"] = statuses.Select(c => new SelectListItem { Text = c.SatusName, Value = c.StatusID.ToString() });
           

        }
		public async Task<IActionResult> ProductDetails(int id)
		{
			Cls_Product cls_Product = new Cls_Product();
            var products = await cls_Product.ProductDetails(id);
            return View(products);
            return View();
		}
		public async Task<IActionResult> ProductIndex()
		{
            SupplierFill();
            StatusFill();
            CategoryFill("all");
            Cls_Product cls_Product = new Cls_Product();
			List<Products> lst = await cls_Product.ProductSelect();
			return View(lst);
		} 
	    public async Task<IActionResult> ProductEdit(int id)
		{
            SupplierFill();
            StatusFill();
            CategoryFill("all");
			
            Cls_Product cls_Product = new Cls_Product();
            if (id == null || context.statuses == null)
            {
                return NotFound();
            }
            var products = await cls_Product.ProductDetails(id);
            return View(products);
        }
		[HttpPost]
		public async Task<IActionResult>ProductEdit(Products products)
		{
            //SupplierFill();
            //StatusFill();
            //CategoryFill("all");
            Products product = context.products.Where(s => s.ProductID == products.ProductID).FirstOrDefault();

			products.HighLighted = product.HighLighted;
			products.AddDate = product.AddDate;
			products.Related = product.Related;
			if (products.PhotoPath == null)
			{
				string? photoPath = context.products.FirstOrDefault(s => s.ProductID == product.ProductID).PhotoPath;
				products.PhotoPath = photoPath;
			}

            Cls_Product cls_Product = new Cls_Product();
            bool answer = cls_Product.ProductUpdate(products);
            if (answer == true)
            {
                TempData["Message"] = products.ProductName.ToUpper() + "Güncellendi";
                return RedirectToAction("ProductIndex");

            }
            else
            {
                TempData["Message"] = "Hata";
            }
            return RedirectToAction("ProductIndex");

        }
        public async Task<IActionResult> ProductDelete(int id)
        {

            if (id == null && context.products != null)
            {
                return NotFound();
            }
			Cls_Product cls_Product = new Cls_Product();
			Products products1 = await cls_Product.ProductDetails(id);
            bool durum = await cls_Product.ProductDelete(id);
            if (durum == true)
            {
                TempData["Message"] = products1.ProductName.ToUpper() + "Güncellendi";
                return RedirectToAction("ProductIndex");
                
            }
			else
			{
                TempData["Message"] = "Hata";
            }
            return View();

        }
    }
	





}
