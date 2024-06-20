namespace SahinTürkEticaretCore.Models
{
    public class MainPageViewModel
    {
        //public List<Products> Product_AdagöreSirala { get; set; }
        //public List<Products> Product_FiyatagöreSirala { get; set; }
        //public int UrunAdet { get; set; }
        //public Products product_detay { get; set; }
        //public List<Products> bunabakanlar { get; set; }
        public List<Products>?SliderProducts { get; set; }
        public List<Products>?NewProduct { get; set; }
        public Products? ProductOfDay { get; set; }
        public List<Products>? Specialproducts { get; set; }
        public List<Products>? Discounts { get; set; }
        public List<Products>? HeighlightedProducts { get; set; }//Öne Çıkan ürünler
        public List<Products> TopSellersProducts { get; set; }
        public List<Products>? Opportunityproducts{ get; set; }
        public List<Products>?StarsProducts { get; set; }
        public List<Products>? NotableProducts { get; set; }
        public Products ProductDetails { get; set; }
        public string? BrandName { get; set; }
        public string? CategoryName { get; set; }
        public List<Products>? RelatedProducts { get; set; }



	}
}
