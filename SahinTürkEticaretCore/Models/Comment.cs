using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SahinTürkEticaretCore.Models
{
	public class Comment
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int CommentID { get; set; }
		public int UserID { get; set; }
		public int ProductID { get; set; }
		[Range(10, 150)]
		[DisplayName("Yorum")]
		public int Review { get; set; }

	}
}
