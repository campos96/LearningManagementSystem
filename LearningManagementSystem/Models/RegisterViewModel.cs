using Microsoft.Build.Framework;

namespace LearningManagementSystem.Models
{
	public class RegisterViewModel : User
	{
		[Required]
		public string Password { get; set; }

		[Required]
		public string PasswordConfirmation { get; set; }

		public bool RememberMe { get; set; }
	}
}
