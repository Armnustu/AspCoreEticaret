using Microsoft.AspNetCore.Mvc;
using SahinTürkEticaretCore.Models;

namespace SahinTürkEticaretCore.ViewComponents
{
    public class Telefon:ViewComponent
    {
        DataContext context = new DataContext(); 
        public string Invoke()
        {
            string Telefon = context.settings.FirstOrDefault(s => s.SettingID == 1).telephone;
            return $"{Telefon}";
        }
    }
}
