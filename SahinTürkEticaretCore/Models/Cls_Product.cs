using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using XAct;

namespace SahinTürkEticaretCore.Models

{
    public class Cls_Product
    {
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public int mainpageCount { get; set; }       
        public int SubPageCount { get; set; }
        public string MyChart { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public string PhotoPath { get; set; }
        public string? Notes { get; set; }
        DataContext context = new DataContext();
         public async Task<List<Products>> ProductSelect() 
        {
            return  await context.products.ToListAsync();
        }
       
        public async Task<bool> Heighlighted_Increase(int id)
        {
            using (DataContext context = new DataContext())
            {
                Products? product = context.products.FirstOrDefault(p => p.ProductID == id);
                product.HighLighted += 1;
                context.Update(product);
                context.SaveChanges();
                return true;
            }
        }
       
        public async Task<List<Products>> ProductSelectSlider(string mainPage,int PageNumber)
		{

            List<Products> products = new List<Products>();
            if (mainPage == "Slider")
            {
                products = await context.products.Where(s => s.StatusID == 2).Take(mainpageCount).ToListAsync();
            }
            else if (mainPage == "NewProduct")
            {
                //ilk anasayfa açıldığında PageNumber -1 değeri gidince gelen ürünler
                if (PageNumber < 0)
                {
                    products = await context.products.OrderByDescending(s => s.AddDate).Take(mainpageCount).ToListAsync();//12 tane ürün gelir
                }
                else//alt sayfa
                {   //alt sayfa ilk tıklama
                    if (PageNumber == 0)
                    {
                        products = context.products.OrderByDescending(p => p.AddDate).Take(SubPageCount).ToList();//4 tane ürün gelir
                    }                                                                     //4*2=8 'den altla 4'ünü al//artarak işlem gerçekleşiyor
                    else                                                                  //4*1=4 'den atla 4'ünü al
                    {                                                                     //4*0=0=0 dan altla 4'nü al
                        products = context.products.OrderByDescending(p => p.AddDate).Skip(SubPageCount * (PageNumber)).Take(SubPageCount).ToList();
                    }
                    //else kısmı buton tıklanmayla gerçekleşen işlem
                    //Pagenumber pageno bağlı olarak yani butona tıklanmaya bağlı olarak artan; Pagenumber sayısına bağlı olarak gelen ürünler                  
                }
            }
            else if (mainPage == "Special")
            {
                
                if (PageNumber < 0)//anasayfa
                {
                    products = await context.products.Where(s => s.StatusID==4).Take(mainpageCount).ToListAsync();//12 tane ürün gelir
                }
                else
                    if (PageNumber == 0)//Alt sayfa ilk tıklama, Önce 0 değeriyle 4 ürün listelenir
                    {
                        products =await context.products.Where(p => p.StatusID==4).Take(SubPageCount).ToListAsync();//4 tane ürün gelir
                    }                                                                      
                    else  //ajax                                                                  
                    {                                                                    
                      products =  await context.products.Where(p => p.StatusID==4).Skip((SubPageCount * (PageNumber))).Take(SubPageCount).ToListAsync();
                    }
                                     
                }

            else if (mainPage == "Discounted")
            {
                if (PageNumber < 0) //ana sayfa
                {
                    products = context.products.OrderByDescending(p => p.Discount).Take(mainpageCount).ToList();
                }
                else //alt sayfa
                {
                    if (PageNumber == 0) //alt sayfa ilk tıklama
                    {
                        products = context.products.OrderByDescending(p => p.Discount).Take(SubPageCount).ToList();
                    }
                    else   //ajax 
                    {
                        products = context.products.OrderByDescending(p => p.Discount).Skip(SubPageCount * (PageNumber)).Take(SubPageCount).ToList();
                    }
                }
            }
            else if (mainPage == "Highligted")
            {


                if (PageNumber < 0)
                {
                    products = await context.products.OrderByDescending(p => p.HighLighted).Take(mainpageCount).ToListAsync();
                }
                else//alt sayfa 
                {
                    if (PageNumber == 0)
                    {
                        products = await context.products.OrderByDescending(p => p.HighLighted).Take(SubPageCount).ToListAsync();
                    }
                    else//ajax
                    {
                        products = await context.products.OrderByDescending(p => p.HighLighted).Take(SubPageCount * (PageNumber)).Take(SubPageCount).ToListAsync();
                    }
                }

                //  products = await context.products.Where(s=>s.StatusID==4).Take(mainpageCount).ToListAsync();
            }
            else if (mainPage == "TopSelllerProducts")
            {
                products = await context.products.OrderByDescending(s => s.TopSeller).Take(mainpageCount).ToListAsync();
            }
            else if (mainPage == "StarsProducts")
            {
                products = await context.products.OrderByDescending(s => s.StatusID == 5).Take(mainpageCount).ToListAsync();
            }
            else if (mainPage == "Opportunityproducts")
            {
                products = await context.products.OrderByDescending(s => s.StatusID == 6).Take(mainpageCount).ToListAsync();
            }
            else if (mainPage == "NotableProducts")
            {
                products = await context.products.OrderByDescending(s => s.StatusID == 10).Take(mainpageCount).ToListAsync();
            }
            else
            {
                return await context.products.ToListAsync();
            }
            return products;

		}
		public async Task<Products> ProductDetails(int? id)
        {
            Products products;
            if (id == 0)
            {
                products = await context.products.FirstOrDefaultAsync(s => s.StatusID == 9);
            }
            else
            {
		       products = await context.products.FindAsync(id);
			}
      
            return products;
        }
        public bool ProductUpdate(Products products)
        {

            context.products.Update(products);
            int affect = context.SaveChanges();
            if (affect > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> ProductInsert(Products product)
        {
                      
            context.products.Add(product);
            int affect = context.SaveChanges();
            bool durum = affect > 0 ? true : false;
            return durum;
        }
        public async Task<bool> ProductDelete(int id)
        {
            try
            {
                using (DataContext context = new DataContext())
                {
                    Products? products = context.products.FirstOrDefault(c => c.ProductID == id);
                    products.Active = false;
                    context.SaveChanges();
                    return true;
                }

            }
            catch (Exception)
            {

                return false;
            }
        }
        public async Task<List<Products>>ProductSelectWithCategory(int id)
        {
            List<Products> lst = await context.products.Where(c => c.CategoryID == id).OrderBy(c => c.ProductName).ToListAsync();
            return lst;
        }
        public async Task<List<Products>> ProductSelectWithSupplier(int id)
        {
            List<Products> lst = await context.products.Where(c => c.SupplierID == id).OrderBy(c => c.ProductName).ToListAsync();
            return lst;
        }

        public List<Cls_Product> products = new List<Cls_Product>();
        SqlConnection sqlconnection = Connection.ServerConnect;
        public List<Cls_Product> SelectProductByDetails(string query)
        {
            List<Cls_Product> products = new List<Cls_Product>();
            SqlConnection sqlconnection = Connection.ServerConnect;
            SqlCommand sqlCommand = new SqlCommand(query,sqlconnection);
            sqlconnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Cls_Product product = new Cls_Product();
                product.ProductID = Convert.ToInt32(sqlDataReader["ProductID"]);
                product.ProductName = sqlDataReader["ProductName"].ToString();
                product.UnitPrice = Convert.ToDecimal(sqlDataReader["UnitPrice"]);
                product.PhotoPath = sqlDataReader["PhotoPath"].ToString();
                product.Notes = sqlDataReader["Notes"].ToString();
                products.Add(product);

            }
            return products;
        }


    }
}
