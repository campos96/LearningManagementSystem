using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningManagementSystem.Models
{
	[Index(nameof(LocationId), nameof(Name), IsUnique = true, Name = "UX_LocationFacility")]
	public class Facility
	{
		[Key]
		public Guid Id { get; set; }

		[Required]
		public Guid LocationId { get; set; }

		[Required]
		public string Name { get; set; }

		public string Address { get; set; }

		[ForeignKey(nameof(LocationId))]
		public virtual Location? Location { get; set; }
	}
}
