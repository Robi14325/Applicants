using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Applicants.Models
{
    public class Applicant
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int Age { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }
        [NotMapped]
        public IFormFile Image {  get; set; }
        public virtual List<Experience> Experience { get; set; } = new List<Experience>();
    }
}
