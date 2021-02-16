using Microsoft.AspNet.Identity.EntityFramework;
using University.BL.Models;
using System.Data.Entity;

namespace University.BL.Data
{
    public class UniversityContext : IdentityDbContext<ApplicationUser>
    {
        public UniversityContext() : base("UniversityContext")
        {
                
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<CourseInstructor> CourseInstructors { get; set; }
        public DbSet<OfficeAssignment> OfficeAssignments { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        public static UniversityContext Create()
        {
            return new UniversityContext();
        }

    }
}
