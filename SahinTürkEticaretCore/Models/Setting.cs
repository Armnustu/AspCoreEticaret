using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SahinTürkEticaretCore.Models
{
	public class Setting
	{

		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int SettingID { get; set; }
		public string? telephone { get; set; }
		[EmailAddress]
		public string Email { get; set; }
		public string? Address { get; set; }
		public int MainpageCount { get; set; }
		public int SubpageCount { get; set; }
	}
}
