using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SahinTürkEticaretCore.Models
{
	public class Status
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int StatusID { get; set; }

		[StringLength(100)]
		[Required]
		public string? SatusName { get; set; }

		public bool Active { get; set; }
	}
}
