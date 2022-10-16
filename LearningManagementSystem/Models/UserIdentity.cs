

using System.ComponentModel.DataAnnotations;

namespace LearningManagementSystem.Models
{
	public class UserIdentity
	{
		[Key]
		[Required]
		public Guid UserId { get; set; }

		[Required]
		public string Password { get; set; }

		public DateTime? PasswordResetDate { get; set; }

		public DateTime? LastLoginDate { get; set; }
	}
}