using Microsoft.EntityFrameworkCore;

namespace OrdersApp.Models
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions options) : base(options)
		{

		}
		public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Order> Orders { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Customer>().HasData
			( new Customer
			  {
				Id = 1,
				Name = "Mahmoud",
				Address = "Tanta",
				Email = "dodomado2015@gmail.com"
			  },
			  new Customer
			  {
				  Id = 2,
				  Name = "Mostafa",
				  Address = "Cairo",
				  Email = "mostafa@gmail.com"
			  }
			);
			modelBuilder.Entity<Product>().HasData
			(
				new Product
				{
					Id = 1,
					Name = "T-shirt",
					Description = "One of the best types of T-shirts",
					Price = 9.99m,
					StockQuantity = 30
				}
				,
				new Product
				{
					Id = 2,
					Name = "Shoes",
					Description = "One of the best types of Shoes",
					Price = 5.99m,
					StockQuantity = 10
				}
			);

		}


	}
}
