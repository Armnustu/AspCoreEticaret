using Microsoft.AspNetCore.Mvc;
using SahinTürkEticaretCore.Models;

namespace SahinTürkEticaretCore.ViewComponents
{
    public class Adress:ViewComponent
    {
        DataContext context = new DataContext();
        public string Invoke()
        {
            string Adress = context.settings.FirstOrDefault(s => s.SettingID == 1).Address;
            return $"{Adress}";
        }
    }
}
