using Microsoft.EntityFrameworkCore;
using Limania.Models;

namespace Limania.DataAccess.Data;

public class ApplicationDbContext : DbContext
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
	{

	}

	public DbSet<Category> Categories { get; set; }

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
	}

}
