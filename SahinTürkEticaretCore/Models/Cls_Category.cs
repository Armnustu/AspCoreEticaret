using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace SahinTürkEticaretCore.Models
{
	public class Cls_Category
	{

		DataContext context = new DataContext();
		public List<Category> CategorySelect(string main_or_all)
		{
			List<Category> categories = new List<Category>();
			if (main_or_all == "all")
			{
				 categories = context.categories.ToList();
			}
			else
			{
                 categories = context.categories.Where(c => c.ParentID == 0).ToList();
            }
				return categories;
		}
		public List<Category> CategorySelectMain()
		{ 
	     List<Category> categories2 = context.categories.Where(c => c.ParentID == 0).ToList();
		return categories2;
	    }
		public bool CategoryInsert(Category category)
		{
			if (category.ParentID == null)
			{
				category.ParentID = 0;
			}

			context.categories.Add(category);
		    int affect=	context.SaveChanges();
			bool durum= affect> 0 ? true : false;
			return durum;
		}
		public bool CategoryUpdate(Category category) {

		  	context.categories.Update(category);
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
		public static bool CategoryDelete(int id)
		{
			try
			{
				using (DataContext context=new DataContext())
				{
					Category? category = context.categories.FirstOrDefault(c => c.CategoryID == id);
					category.Active = false;
					List<Category> categoriList = context.categories.Where(c => c.ParentID == id).ToList();
					foreach (var item in categoriList)
					{
						item.Active = false;
					}
					context.SaveChanges();
					return true;
				}

			}
			catch (Exception)
			{

				return false;
			}
		}

		 public async Task<Category> CategoryDetails(int? id)
		{
			Category? categories = await context.categories.FindAsync(id);
			return categories;
		}
	}
}
