using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SahinTürkEticaretCore.Models
{
	public class Products
	{
		[Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductID { get; set; }
		public string? ProductName { get; set; }

		public decimal UnitPrice { get; set; }
		[DisplayName("Kategori")]
		public int CategoryID { get; set; }
		private int _Kdv { get; set; }
		public int Kdv { get { return _Kdv; } set { _Kdv = Math.Abs(value); } }
		[DisplayName("Marka")]
		public int SupplierID { get; set; }
		public int Stock { get; set; }
		public int Discount { get; set; }

		[DisplayName("Statüs")]
		public int StatusID { get; set; }

		public DateTime AddDate { get; set; }
		public string? Keywords { get; set; }

		public int HighLighted { get; set; }//Öne çıkanlar
		public int TopSeller { get; set; }//Çok Satanlar
		[DisplayName("Buna bakanlar")]
		public int Related { get; set; }//Buna Bakanlar
		public string? Notes { get; set; }
		public string? PhotoPath { get; set; }
		[DisplayName("Aktif")]
		public bool Active { get; set; }







	}
}
