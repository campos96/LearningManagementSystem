using Microsoft.EntityFrameworkCore;
using LearningManagementSystem.Models;

namespace LearningManagementSystem.Data
{
	public class LMSContext : DbContext
	{
		public LMSContext(DbContextOptions<LMSContext> options) : base(options)
		{

		}

		public DbSet<User> Users { get; set; } = default!;

		public DbSet<UserIdentity> UserIdentities { get; set; } = default!;

		public DbSet<Company> Companies { get; set; } = default!;

		public DbSet<Location> Locations { get; set; } = default!;

		public DbSet<Facility> Facilities { get; set; } = default!;

		public DbSet<Department> Departments { get; set; } = default!;

	}
}
