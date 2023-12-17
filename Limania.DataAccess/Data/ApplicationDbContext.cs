using Microsoft.EntityFrameworkCore;
using Limania.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Limania.DataAccess.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
	{

	}

	public DbSet<Category> Categories { get; set; }
	public DbSet<Product> Products { get; set; }
	public DbSet<ApplicationUser> ApplicationUsers { get; set; }
	public DbSet<ShoppingCart> ShoppingCarts { get; set; }
	public DbSet<Company> Companies { get; set; }
	public DbSet<OrderHeader> OrderHeaders { get; set; }
	public DbSet<OrderDetail> OrderDetails { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{

		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<Category>().HasData(
		new Category { Id = 1, Name = "Aksiyon", DisplayOrder = 1 },
		new Category { Id = 2, Name = "Bilim Kurgu", DisplayOrder = 2 },
		new Category { Id = 3, Name = "Macera", DisplayOrder = 3 },
		new Category { Id = 4, Name = "Komedi", DisplayOrder = 4 },
		new Category { Id = 5, Name = "Romantik", DisplayOrder = 5 },
		new Category { Id = 6, Name = "Korku", DisplayOrder = 6 },
		new Category { Id = 7, Name = "Gerilim", DisplayOrder = 7 },
		new Category { Id = 8, Name = "Çocuk", DisplayOrder = 8 },
		new Category { Id = 9, Name = "Yetişkin", DisplayOrder = 9 }
					);
		modelBuilder.Entity<Company>().HasData(
			new Company { Id = 1, Name = "Kitap Yurdu", City = "İstanbul", State = "Kartal", StreetAddress = "A Sokak B Mahallesi 25/1", PhoneNumber = "05555555555", PostalCode = "34000" },
			new Company { Id = 2, Name = "Kitap Seç", City = "İstanbul", State = "Taksim", StreetAddress = "T Sokak C Mahallesi 22/5", PhoneNumber = "05544444444", PostalCode = "34000" }

			);
		modelBuilder.Entity<Product>().HasData(

			new Product
			{
				Id = 1,
				Tittle = "Nazım İle Piraye",
				Description = "Öyle ki bir mektubunda Piraye'ye 'Sen benim en yakın insanımsın. ' diyor Nazım.",
				Author = "Nazım Hikmet",
				ISBN = "SWD9999001",
				ListPrice = 99,
				Price = 90,
				Price50 = 85,
				Price100 = 80,
				CategoryId = 5,
				ImageUrl = ""
			},
			new Product
			{
				Id = 2,
				Tittle = "Hanımın Çiftliği",
				Description = "Hanımın Çiftliği, Orhan Kemal'in üç ciltlik romanıdır. Kitap, Vukuat Var, Hanımın Çiftliği ve Kaçak ciltlerinden oluşmaktadır",
				Author = "Orhan Kemal",
				ISBN = "SWD9999002",
				ListPrice = 105,
				Price = 100,
				Price50 = 95,
				Price100 = 90,
				CategoryId = 9,
				ImageUrl = ""
			}
			);
	}

}
