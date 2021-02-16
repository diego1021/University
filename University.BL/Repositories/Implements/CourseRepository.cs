using System.Threading.Tasks;
using University.BL.Data;
using University.BL.Models;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;

namespace University.BL.Repositories.Implements
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        private readonly UniversityContext universityContext;
        public CourseRepository(UniversityContext universityContext) : base(universityContext)
        {
            this.universityContext = universityContext;
        }

        public async Task<bool> DeleteCheckOnEntity(int id)
        {
            var flag = await universityContext.CourseInstructors.Where(x => x.CourseID == id).AnyAsync();
            return flag;
        }

        public async Task<IEnumerable<Student>> GetStudentsByCourses(int id)
        {
            //var courses = universityContext.CourseInstructors
            //    .Include("Course")
            //    .Where(x => x.InstructorID == id)
            //    .Select(x => x.Course);

            var students = (from enrollment in universityContext.Enrollments
                           join student in universityContext.Students
                           on enrollment.StudentID equals student.ID
                           where enrollment.CourseID == id
                           select student);

            return await students.ToListAsync();
        }
    }
    
}
