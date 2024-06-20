using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SahinTürkEticaretCore.Models
{
	public class Supplier
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int SupplierID { get; set; }
		[StringLength(100)]
		[Required]
		[DisplayName("Kategori Adı")]
		public string? BrandName { get; set; }
		[Required]		
		public string? PhotoPath { get; set; }
		public bool Active { get; set; }
	}
}
