using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningManagementSystem.Models
{
	[Index(nameof(CompanyId), nameof(City), nameof(State), nameof(Country), IsUnique = true, Name = "UX_CompanyLocation")]
	public class Location
	{
		[Key]
		public Guid Id { get; set; }

		[Required]
		public Guid CompanyId { get; set; }

		[Required]
		public string City { get; set; }

		[Required]
		public string State { get; set; }

		[Required]
		public string Country { get; set; }

		[ForeignKey(nameof(CompanyId))]
		public virtual Company? Company { get; set; }
	}
}
