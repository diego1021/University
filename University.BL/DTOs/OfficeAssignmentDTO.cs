using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace University.BL.DTOs
{
    public class OfficeAssignmentDTO
    {
        [Required]
        [Display(Name = "Instructors")]
        public int InstructorID { get; set; }

        [Required]
        [Display(Name = "Location")]
        [StringLength(50)]
        public string Location { get; set; }

        public InstructorDTO Instructor { get; set; }
    }
}
