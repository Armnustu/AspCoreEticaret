using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SahinTürkEticaretCore.Models
{
	public class Category
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int CategoryID { get; set; }
		
		[DisplayName("Üst Kategori")]
		public int ParentID { get; set; }
		[StringLength(50, ErrorMessage = "Enfazla 50 karakter girebilirsiniz")]
		[Required(ErrorMessage = "Kategori Alan Zorunlu")]
		[DisplayName("Kategori Adı")]
		public string CategoryName { get; set; }
		[DisplayName("Aktif/Pasif")]
		public bool Active { get; set; }

	}
}
