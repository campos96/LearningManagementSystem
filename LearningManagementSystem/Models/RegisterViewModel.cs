using System.ComponentModel.DataAnnotations;

namespace LearningManagementSystem.Models
{
	public class RegisterViewModel : User
	{
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required]
		[Compare("Password")]
		[DataType(DataType.Password)]
		[Display(Name = "Password Confirmation")]
		public string PasswordConfirmation { get; set; }

		public bool RememberMe { get; set; }
	}
}
