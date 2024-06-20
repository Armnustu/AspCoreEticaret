using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SahinTürkEticaretCore.Models
{
	public class Activite
	{
		[Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ActiviteID { get; set; }
		public string Tur { get; set; }
		public string Adi { get; set; }
		public DateTime EtkinlikBaslamaTarihi { get; set; }
		public DateTime EtkinlikBitisTarihi { get; set; }
		public string EtkinlikMerkezi { get; set; }
		public string KisaAciklama { get; set; }
		public string BiletSatisLinki { get; set; }
		public string KücükAfis { get; set; }
	}
}
