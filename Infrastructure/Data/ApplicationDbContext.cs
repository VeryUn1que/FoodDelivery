using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ApplicationCore.Models;

namespace Infrastructure.Data
{
	public class ApplicationDbContext : IdentityDbContext
	{
#pragma warning disable CS8618
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
#pragma warning restore CS8618
			: base(options)
		{
		}

		public DbSet<Category> Category { get; set; }
		
		 
	}
}
