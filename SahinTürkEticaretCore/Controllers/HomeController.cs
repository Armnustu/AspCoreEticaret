using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using SahinTürkEticaretCore.Models;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using X.PagedList;
using XAct;

namespace SahinTürkEticaretCore.Controllers
{
   // 1-Soru [httpget] Confirmpage sayfası
   //2-.ChartProcess
   //3-websitesinde cookieye nerden bakıyoruz
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        MainPageViewModel mainPageViewModel = new MainPageViewModel();
        Cls_Product cls_Product = new Cls_Product();
        Cls_Order cls_Order = new Cls_Order();
        DataContext context = new DataContext();
        //Bütün  ürünler gelir
        public async Task<IActionResult> Index()
        {

            //string status = "";
            //status = Request.Query["status"].ToString();
            //if (status != null)
            //{
            //    if (status == "1")
            //    {
            //        ViewBag.Status = true;
            //    }
            //}

            cls_Product.mainpageCount = mainpageCount;
            //-1 değeriyle anasayfada önce bütün ürünler listelenir 
            mainPageViewModel.SliderProducts = await cls_Product.ProductSelectSlider("Slider", -1);
            mainPageViewModel.NewProduct = await cls_Product.ProductSelectSlider("NewProduct", -1);
            mainPageViewModel.Specialproducts = await cls_Product.ProductSelectSlider("Special", -1);
            mainPageViewModel.Discounts = await cls_Product.ProductSelectSlider("Discounted", -1);
            mainPageViewModel.HeighlightedProducts = await cls_Product.ProductSelectSlider("Heightlighted", -1);
            mainPageViewModel.TopSellersProducts = await cls_Product.ProductSelectSlider("TopSelllerProducts", -1);
            mainPageViewModel.StarsProducts = await cls_Product.ProductSelectSlider("StarProducts", -1);
            mainPageViewModel.Opportunityproducts = await cls_Product.ProductSelectSlider("Opportunityproducts", -1);
            mainPageViewModel.NotableProducts = await cls_Product.ProductSelectSlider("NotableProducts", -1);
            mainPageViewModel.ProductOfDay = await cls_Product.ProductDetails(0);
            return View(mainPageViewModel);
        }
        //Microsoft.AspNetCore.Http
        public async Task<IActionResult> ChartProcess(int id)
        {
            bool status = await cls_Product.Heighlighted_Increase(id);
            if (status = true)
            {
                cls_Order.ProductID = id;
                cls_Order.Quantity = 1;
                var cookieOptions = new CookieOptions();
                var cookie = Request.Cookies["Sepetim"];//tarayıcıda oku
                if (cookie == null)
                {

                    cookieOptions.Expires = DateTime.Now.AddDays(1);                    
                    cookieOptions.Path = "/";
                    cls_Order.MyCart = "";//sepetim properties
                    cls_Order.AddToMyCart(id.ToString());
                    Response.Cookies.Append("Sepetim", cls_Order.MyCart, cookieOptions);//ilk ürün ekleme işlemi yapılr cookiye yapılır
                    TempData["Message"] = "ürün sepete eklendi";
                }
                else
                {
                    cls_Order.MyCart = cookie;//yukarıda apend edilmiş birde nediye eşitliyoruz x=y  sonra tekrar y=x yapmış?????????????
                    //Sepete eklenmişse if altındaki işlemleri yap
                    if (cls_Order.AddToMyCart(id.ToString()) == false)
                    {
                        //Cls_Order.Mychart eklenen veri Sepetime ekleniyor yani cookie ekleniyor
                        HttpContext.Response.Cookies.Append("Sepetim", cls_Order.MyCart, cookieOptions);
                        cookieOptions.Expires = DateTime.Now.AddDays(1);
                        TempData["Message"] = "ürün sepete eklendi";

                    }
                    else//eklenmişse ürün daha önce eklenmiştir diyecek
                    {

                        TempData["Message"] = "ürün sepetenizde zaten var";
                    }

                }
            }
            string url = Request.Headers["Referer"].ToString();
            return Redirect(url);
///
        }
        //public async Task<IActionResult> Details(int id)//
        //{
        //    await cls_Product.Heighlighted_Increase(id);//Highligted ürüne birdefa ürün adedini artırıyoruz
        //    return View();
        //}      
        public IActionResult Details(int id)
        {

            mainPageViewModel.ProductDetails = (from p in context.products where p.ProductID == id select p).FirstOrDefault();

            //linq
            mainPageViewModel.CategoryName = (from p in context.products
                                              join c in context.categories
                                              on p.CategoryID equals c.CategoryID
                                              where p.ProductID == id
                                              select c.CategoryName).FirstOrDefault();

            //linq
            mainPageViewModel.BrandName = (from p in context.products
                                           join s in context.suppliers
                                           on p.SupplierID equals s.SupplierID
                                           where p.ProductID == id
                                           select s.BrandName).FirstOrDefault();

            //select * from Products where Related = 2 and ProductID != 4
            mainPageViewModel.RelatedProducts = context.products.Where(p => p.Related == mainPageViewModel.ProductDetails!.Related && p.ProductID != id).ToList();

            //cls_Product.Highlighted_Increase(id);

            return View(mainPageViewModel);
        }
        public async Task<IActionResult> TopSellerProducts(int page = 1)
        {

            var model =  context.products.OrderByDescending(p => p.TopSeller).ToPagedList(page, 8);
            return View("TopSellerProducts", model);
        }

        int SubPageCount = 0;
        public HomeController()
        {
            this.mainpageCount = context.settings.FirstOrDefault(s => s.SettingID == 1).MainpageCount;
            this.SubPageCount = context.settings.FirstOrDefault(s => s.SettingID == 1).SubpageCount;
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

        }
        //alt sayfa ajax yaparken
        public async Task<PartialViewResult> _PartialNewProduct(string pageNo)
        {
            cls_Product.SubPageCount = SubPageCount;
            int pageNumber = Convert.ToInt32(pageNo);//
            mainPageViewModel.NewProduct = await cls_Product.ProductSelectSlider("NewProduct", pageNumber);
            return PartialView(mainPageViewModel);
        }

        //javascripten gelen değer pageno
        public async Task<PartialViewResult> _PartialSpecialProduct(int pageNo)
        {
            cls_Product.SubPageCount = SubPageCount;//Home controllerda yapıcı metodla değeri alır
            int pageNumber = Convert.ToInt32(pageNo);
            mainPageViewModel.Specialproducts = await cls_Product.ProductSelectSlider("Special", pageNumber);
            return PartialView(mainPageViewModel);
        }
        //alt sayfa ajax yaparken
        public async Task<IActionResult> SpecialProducts()
        {
            cls_Product.SubPageCount = SubPageCount;
            mainPageViewModel.Specialproducts = await cls_Product.ProductSelectSlider("Special", 0);//0 değeriyle önce 4 ürün listelenir
            return View(mainPageViewModel);
        }

        int mainpageCount = 0;
        //alt sayfaya ilk tıklanınca
        //New product sayfası önce 0 değeri gittiği için 4tane değer gelir
        public async Task<IActionResult> NewProducts()//yeni ürünler sayfasına tıklandığında yapılması gereken işlemler 
        {
            cls_Product.SubPageCount = SubPageCount;
            cls_Product.mainpageCount = mainpageCount;
            mainPageViewModel.NewProduct = await cls_Product.ProductSelectSlider("NewProduct", 0);//önce 0 değeri gönderilir 4 ürün gelmesi için 
            return View(mainPageViewModel);
        }

        //sanra buton değerinin artışına bağlı olarak 
        public async Task<PartialViewResult> _PartialDiscountProduct(string pageNo)
        {
            cls_Product.SubPageCount = SubPageCount;
            int pageNumber = Convert.ToInt32(pageNo);
            mainPageViewModel.Discounts = await cls_Product.ProductSelectSlider("Discounted", pageNumber);
            return PartialView(mainPageViewModel);
        }
        //Bu sayfada ürünleri listeler
        public async Task<IActionResult> DiscountProducts()
        {
            cls_Product.SubPageCount = SubPageCount;
            mainPageViewModel.Discounts = await cls_Product.ProductSelectSlider("Discounted", 0);
            return View(mainPageViewModel);
            return View();
        }
        public async Task<PartialViewResult> _PartialHighletedProduct(string pageNo)
        {
            cls_Product.SubPageCount = SubPageCount;
            int pageNumber = Convert.ToInt32(pageNo);
            mainPageViewModel.HeighlightedProducts = await cls_Product.ProductSelectSlider("Highligted", pageNumber);
            return PartialView(mainPageViewModel);
        }
        public async Task<IActionResult> HighligtedProducts()//Bu sayfada ürünleri listeler
        {
            cls_Product.SubPageCount = SubPageCount;
            mainPageViewModel.HeighlightedProducts = await cls_Product.ProductSelectSlider("Highligted", 0);
            return View(mainPageViewModel);
            return View();
        }
        Cls_Category cls_Category = new Cls_Category();
        public async Task<IActionResult> CategoryPage(int id)
        {
            List<Products> lst = await cls_Product.ProductSelectWithCategory(id);
            Category category = await cls_Category.CategoryDetails(id);
            ViewBag.Header = category.CategoryName;
            return View(lst);
        }
        Cls_Supplier cls_Supplier = new Cls_Supplier();
        public async Task<IActionResult> SupplierPage(int id)
        {
            List<Products> lst = await cls_Product.ProductSelectWithSupplier(id);
            Supplier supplier = await cls_Supplier.supplierDetails(id);
            ViewBag.Header = supplier.BrandName;
            return View(lst);
        }
        public async Task<IActionResult> Cart()
        {
            List<Cls_Order> sepet;
           string? scid = HttpContext.Request.Query["scid"];

            if (HttpContext.Request.Query["scid"].ToString() != "")//Silme işlemi için yapıldı
            {
                //sil botunuyla geldim
               // string? scid = HttpContext.Request.Query["scid"];
                cls_Order.MyCart = Request.Cookies["sepetim"]; //tarayıcıdan al , property ye yaz
                cls_Order.DeleteFromMyCart(scid); //property den sildim
                var cookieOptions = new CookieOptions();
                //tarayıcıya silnmiş halini (güncel) gönderdim
                Response.Cookies.Append("sepetim", cls_Order.MyCart, cookieOptions);
                cookieOptions.Expires = DateTime.Now.AddDays(1); //1 günlük çerez
                TempData["Message"] = "Ürün Sepetten Silindi";
                //sepet içindeki son haline cart.cshtml sayfasına da gönderecegim
                sepet = cls_Order.SelectMyCart();
                ViewBag.Sepetim = sepet;
                ViewBag.sepet_tablo_detay = sepet;
            }
            else  //cookiedeki verileri alıp listeleyip gönderir
            { 
                 scid = HttpContext.Request.Query["scid"];
                //sag üst köseden sepet tıklanınca
                var cookie = Request.Cookies["sepetim"];
                if (cookie == null)
                {
                    cls_Order.MyCart = "";
                    sepet = cls_Order.SelectMyCart();//listeleme işlemi yapar
                    ViewBag.Sepetim = sepet;
                    ViewBag.sepet_tablo_detay = sepet;
                }
                else
                {

                    var cookieOptions = new CookieOptions();
                    cls_Order.MyCart = Request.Cookies["sepetim"];
                    sepet = cls_Order.SelectMyCart();
                    ViewBag.Sepetim = sepet;
                    ViewBag.sepet_tablo_detay = sepet;
                }
            }
            if (sepet.Count == 0)
            {
                ViewBag.Sepetim = null;
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Order(IFormCollection frm)
        {
            Cls_Order cls_Order = new Cls_Order();
            string txt_individual = Request.Form["txt_individual"];
            string txt_corporate = Request.Form["txt_corporate"];
            if (txt_individual != null)
            {
                //bireyse fatura
                //digital planet
                WebServiceController.tckimlik_vergi_no = txt_individual;
                Cls_Order.tckimlik_vergi_no = txt_individual;
                cls_Order.EfaturaCreate();

            }
            else
            {
                //kurumsal fatura
                WebServiceController.tckimlik_vergi_no = txt_corporate;
                Cls_Order.tckimlik_vergi_no = txt_corporate;
                cls_Order.EfaturaCreate();
            }
            //Kredi kart bilgileri viewden gelir
            string kredikartno = Request.Form["kredikartno"];
            string kredikartay = frm["kredikartay"];
            string kredikartyil = frm["kredikartyil"];
            string kredikartcvs = frm["kredikartcvs"];
            return RedirectToAction("backref");//Siparis onaylandıktan sonra  bacref sayfasına yönlendirilir ve cookie yani sepet bilgileri silinir
            // buradan sonraki kodlar, payu, iyzico
            // return View();//??

        }
        public static string POSTFormPAYU(string url, NameValueCollection data)
        {
            return "";
        }
        public static string HashWithSignature(string deger, string signatureKey)
        {
            return "";
        }
        public IActionResult backref()
        {
            var cookieoptions = new CookieOptions();
            var cookie = Request.Cookies["Sepetim"];
            if (cookie != null)
            {
                cls_Order.MyCart = cookie;
                OderGroupGUID = cls_Order.OrderCreate(HttpContext.Session.GetString("Email").ToString());
                cookieoptions.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Delete("Sepetim");

            }        
            return RedirectToAction("ConfirmPage");
        }
        public static string OderGroupGUID = "";        
        public IActionResult ConfirmPage()//Bu sayfa sipariş numarasını gönderir
        {
            ViewBag.OrderGroupGUID = OderGroupGUID; //OrderGroupGUID değerini nasıl alır kontroller arası doğrudan veride gönderemiyoruz??????????????     
            return View();
        }
        public async Task<IActionResult> Order()
        {
            //HttpContext.Session.SetString("session", "deneme");
            //HttpContext.Session.GetString("email");
            if (HttpContext.Session.GetString("Email") != null)
            {
                //kullanıcı Login.cshtml den giriş yapıp , Session alıp gelmiştir
                User? usr = await Cls_User.SelectMemberInfo(HttpContext.Session.GetString("Email"));//Giriş yaptıktan sonra websitesinden mailini alarak user bilgilerini Order sayfasına gönderir
                return View(usr);
            }
            else
            {
                //kullanıcı Login.cshtml ye gitmemiş , Session alıp gelmemiş
                return RedirectToAction("Login");
            }

        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            string answer = await Cls_User.MemberControl(user);
            if (answer == "error")
            {
                TempData["Message"] = "Email/Şifre yanlış girildi";
                return View();
            }
            else if (answer == "admin")
            {
                HttpContext.Session.SetString("Email", answer);
                HttpContext.Session.SetString("Admin", answer);
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                HttpContext.Session.SetString("Email", answer);
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Email");
            HttpContext.Session.Remove("Admin");
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            if (await Cls_User.LoginEmailControl(user) == false)
            {
                bool answer = await Cls_User.AddUser(user);

                if (answer)
                {
                    TempData["Message"] = "Kaydedildi.";
                    return RedirectToAction("Login");
                }
                TempData["Message"] = "Hata.Tekrar deneyiniz.";
            }
            else
            {
                TempData["Message"] = "Bu Email Zaten mevcut.Başka Deneyiniz.";
            }
            return View();
        }

        public IActionResult ConfirmOrder()
        {
            var cookieOptions = new CookieOptions();
            var cookie = Request.Cookies["Sepetim"];
            if (cookie != null)
            {
                cls_Order.MyCart = cookie;
                OderGroupGUID = cls_Order.OrderCreate(HttpContext.Session.GetString("Email").ToString());
                cookieOptions.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Delete("Sepetim");

            }
            return RedirectToAction("ConfirmPage");
        }
        
        public async Task<IActionResult> MyOrders()
        {
            if (HttpContext.Session.GetString("Email") != null)//Giriş yapıldığında mail boş değilse
            {
                List<Vw_MyOrder> orders =await  cls_Order.SelectMyOrders(HttpContext.Session.GetString("Email").ToString());
                return View(orders);
            }
            else
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        
        public async Task<IActionResult> DetailsSearch()
        {

            ViewBag.Categories = context.categories.ToList();
            ViewBag.Suppliers = context.suppliers.ToList();

            return View();
        }
        public List<Vw_MyOrder>SelectMyOrders(string Email)
        {
            int UserID = context.users.FirstOrDefault(u => u.Email == Email).UserID;
            List<Vw_MyOrder> myorders = context.vm_MyOrders.Where(o => o.UserID == UserID).ToList();
            return myorders;
        }
        //SupplierID gelen her değer dizi değerine atılıyor
        public IActionResult DbProducts(int CategoryID, string[] SupplierID,string price,string IslnStock)
        {
            price = price.Replace(" ", "").Replace("TL", "");//arada boşluk varsa boşlukları kaldır arada TL varsa TL'yi kaldır
            string[] PriceArray = price.Split('-');//sonra arada var olan spliti kaldır iki dizi haline dönüştür ve değerleri aktar
            string startprice = PriceArray[0];
            string endprice = PriceArray[1];
            string sign = ">";
            if (IslnStock == "0")
            {
                sign = ">=";

            }
            int count = SupplierID.Length;
            string suppliervalue = "";
            for (int i = 0; i < SupplierID.Length; i++)
            {
                if (count ==1)//tekmarka varsa
                {
                    suppliervalue = "SupplierID=" + SupplierID[i];//Buraya gelen dizin değerinin valuelarıda SupplierID'ye aktarılıyor
                    count++;
                }
                else//birden fazla marka varsa
                {
                    suppliervalue += "or SupplierID=" + SupplierID[i];//or SupplierID[0]=1 or suppplierID[1]=2 şeklinde gider
                }
            }
            string query = "select * from products where CategoryID="+CategoryID+ "and ("+ suppliervalue + ") and (UnitPrice>="+startprice+" and UnitPrice<="+endprice+") and Stock"+sign+" 0 order by UnitPrice";
            ViewBag.Products = cls_Product.SelectProductByDetails(query);
            return View();
        }

}
}