using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LearningManagementSystem.Models
{
	[Index(nameof(Name), IsUnique = true, Name = "UX_Company")]
	public class Company
	{
		[Key]
		public Guid Id { get; set; }

		[Required]
		public string Name { get; set; }

	}
}
