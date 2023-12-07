using Microsoft.EntityFrameworkCore;

namespace LimaniaStore.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}


	}
}
