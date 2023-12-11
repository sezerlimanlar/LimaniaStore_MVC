using Microsoft.EntityFrameworkCore;
using Limania.Models;

namespace Limania.DataAccess.Data;

public class ApplicationDbContext : DbContext
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
	{

	}

	public DbSet<Category> Categories { get; set; }
	public DbSet<Product> Products { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
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
				ImageUrl=""
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
				ImageUrl=""
			}
			);
	}

}
