using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace SahinTürkEticaretCore.Models
{
	public class DataContext : DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			
			var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
			var configuration=builder.Build();
			optionsBuilder.UseSqlServer(configuration["ConnectionStrings:Connection1"]);


		}
		public DbSet<Activite>? activites { get; set; }
		public DbSet<Category>? categories { get; set; }
		public DbSet<Comment>? comments { get; set; }
		public DbSet<Message>? messages { get; set; }
		public DbSet<User>? users { get; set; }
		public DbSet<Order>? orders { get; set; }		
		public DbSet<Products>? products { get; set; }	
		public DbSet<Setting>? settings { get; set; }
		public DbSet<Supplier>? suppliers { get; set; }
		public DbSet<Status>? statuses { get; set; }
        public DbSet<Vw_MyOrder>?vm_MyOrders{ get; set; }
       
        
    }
}