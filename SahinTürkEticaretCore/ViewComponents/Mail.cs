using Microsoft.AspNetCore.Mvc;
using SahinTürkEticaretCore.Models;

namespace SahinTürkEticaretCore.ViewComponents
{
    public class Mail:ViewComponent
    {
        DataContext context = new DataContext();
        public string Invoke()
        {


            string Mail = context.settings.FirstOrDefault(s => s.SettingID == 1).Email;
            return $"{Mail}";
        }
    }
}
