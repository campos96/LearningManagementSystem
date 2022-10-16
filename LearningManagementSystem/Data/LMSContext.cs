using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

	}
}
