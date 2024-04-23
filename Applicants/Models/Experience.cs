using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Applicants.Models
{
    public class Experience
    {
        public Experience() 
        {

        }
        [Key]
        public int Id { get; set; }
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public int YearOfExperience { get; set; }
        [ForeignKey("Applicant")]
        public int ApplicantID { get; set; }
        public virtual Applicant Applicant { get; private set; }
    }
}
