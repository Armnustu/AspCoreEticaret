using Microsoft.EntityFrameworkCore;

namespace SahinTürkEticaretCore.Models
{
    public class Cls_Status
    {
		DataContext context = new DataContext();
		public async Task<List<Status>>StatusSelect()
		{
			List<Status> lst = await context.statuses.ToListAsync();
			return lst;
		}
		public bool StatusInsert(Status status)
		{
			bool durum = false;
			if (status!= null)
			{
				context.statuses.Add(status);
				int affect = context.SaveChanges();
				 durum = affect > 0 ? true : false;
				return durum;
			}
			return durum;

		}
		public bool StatusUpdate(Status status)
		{

			context.statuses.Update(status);
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
		public async Task<bool> StatusDelete1(int id)
		{
			try
			{
				using (DataContext context = new DataContext())
				{
					Status? status = context.statuses.FirstOrDefault(c => c.StatusID == id);
					status.Active = false;					
					context.SaveChanges();
					return true;
				}

			}
			catch (Exception)
			{

				return false;
			}
		}

		public async Task<Status> StatusDetails(int? id)
		{
			Status? status = await context.statuses.FindAsync(id);
			return status;
		}





	}
}
