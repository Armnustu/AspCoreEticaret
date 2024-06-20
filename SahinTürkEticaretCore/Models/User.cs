using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SahinTürkEticaretCore.Models
{

	public class User
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int UserID { get; set; }
		[StringLength(100)]
		[Required]
		public string NameSurname { get; set; }
		[StringLength(100)]
		
		[EmailAddress]
		[Required(ErrorMessage ="Email zorunlu alan")]
		public string Email { get; set; }
		[StringLength(100)]
		
		[DataType(DataType.Password)]
		[Required(ErrorMessage ="şifre Zorunlu alan")]
		public string Password { get; set; }
		public string? Telephone { get; set; }
		public string InvoicesAddres { get; set; }//Fatura adresi
		public bool? IsAdmin { get; set; }
		public bool Active { get; set; }
		
	}
}
