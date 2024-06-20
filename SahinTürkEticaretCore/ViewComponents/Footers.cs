using Microsoft.AspNetCore.Mvc;
using SahinTürkEticaretCore.Models;

namespace SahinTürkEticaretCore.ViewComponents
{
    public class Footers:ViewComponent
    {
        DataContext context = new DataContext();
        public IViewComponentResult Invoke()
        {
            List<Supplier> suppliers = context.suppliers.Where(s=>s.Active==true).ToList();
            return View(suppliers);
        }
    }
}
