using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace LearningManagementSystem.Models
{
	[Index(nameof(FacilityId), nameof(Number), nameof(Name), IsUnique = true, Name = "UX_FacilityDepartment")]
	public class Department
	{
		[Key]
		public Guid Id { get; set; }

		[Required]
		public Guid FacilityId { get; set; }

		[Required]
		public string Number { get; set; }

		[Required]
		public string Name { get; set; }

		public Guid? ParentDepartmentId { get; set; }

		[ForeignKey(nameof(ParentDepartmentId))]
		public virtual Department? ParentDepartment { get; set; }

		[ForeignKey(nameof(FacilityId))]
		public virtual Facility? Facility { get; set; }

	}
}
