using System;
using System.Collections.Generic;


namespace University.BL.DTOs
{
    public class StudentDTO
    {
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstMidName { get; set; }
        public DateTime EnrollmentDate { get; set; }

        public ICollection<EnrollmentDTO> Enrollments { get; set; }
    }
}
