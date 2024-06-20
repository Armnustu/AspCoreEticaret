using Microsoft.EntityFrameworkCore;
using XAct;
//https://mahedee.net/crud-operation-using-asynchronous-method-in-asp-net-mvc/
namespace SahinTürkEticaretCore.Models
{
    public class Cls_Supplier
    {
        DataContext context = new DataContext();
        public async Task<List<Supplier>> SupplierSelect()
        {
            DataContext context = new DataContext();
            List<Supplier> suppliers = await context.suppliers.ToListAsync();
            return suppliers;
        }
		public async Task<Supplier> supplierDetails(int? id)
		{
			Supplier? suppliers = await context.suppliers.FindAsync(id);
			return suppliers;
		}

		public async Task<bool> SupplierDelete(int? id)
		{
			bool durum=false;
			var supplier = await context.suppliers.FindAsync(id);
			if (supplier != null && id!=null)
			{
				supplier.Active = false;
				supplier.BrandName = supplier.BrandName;
				supplier.PhotoPath = supplier.PhotoPath;
				context.Entry(supplier).State = EntityState.Modified;
		        context.SaveChanges();
				//durum = affect > 0 ? true:false;
				durum = true;
			}
			return durum;	
		
		}

		public bool SupplierInsert(Supplier supplier)
		{
			

			context.suppliers.Add(supplier);
			int affect = context.SaveChanges();
			bool durum = affect > 0 ? true : false;
			return durum;


		}
		public bool SupplierUpdate(Supplier supplier)
		{

			context.suppliers.Update(supplier);
			int affect = context.SaveChanges();
			if (affect > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
