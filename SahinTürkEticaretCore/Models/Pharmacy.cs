namespace SahinTürkEticaretCore.Models
{
	//https://openapi.izmir.bel.tr/api/ibb/nobetcieczaneler
	public class Pharmacy
	{
		public DateTime Tarih { get; set; }
		public string LokasyonY { get; set; }
		public string LokansyonX { get; set; }
		public string Adi { get; set; }
		public string Telefon { get; set; }
		public string Adres { get; set; }
	}
}
