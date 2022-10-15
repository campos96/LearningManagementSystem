using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LearningManagementSystem.Models
{
	[Index(nameof(Username), IsUnique = true, Name = "UX_Username")]
	[Index(nameof(Email), IsUnique = true, Name = "UX_Email")]
	public class User
	{
		[Key]
		public Guid Id { get; set; }

		[Required]
		public string Username { get; set; }

		[Required]
		public string Email { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public string Lastname { get; set; }

		[Required]
		public DateTime CreatedAt { get; set; }

		public DateTime? UpdatedAt { get; set; }

		public DateTime? DisabledAt { get; set; }

		public DateTime? DeletedAt { get; set; }

		public string Fullname => $"{Name} {Lastname}";

	}
}
