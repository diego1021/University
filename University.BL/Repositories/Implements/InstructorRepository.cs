using System.Collections.Generic;
using System.Threading.Tasks;
using University.BL.Data;
using University.BL.Models;
using System.Linq;
using System.Data.Entity;

namespace University.BL.Repositories.Implements
{
    public class InstructorRepository : GenericRepository<Instructor>, IInstructorRepository
    {
        private readonly UniversityContext universityContext;
        public InstructorRepository(UniversityContext universityContext) : base(universityContext)
        {
            this.universityContext = universityContext;
        }

        public async Task<bool> DeleteCheckOnEntity(int id)
        {
            var flag = await universityContext.CourseInstructors.Where(x => x.CourseID == id).AnyAsync();
            return flag;
        }

        public async Task<IEnumerable<Course>> GetCoursesByInstructor(int id)
        {
            //var courses = universityContext.CourseInstructors
            //    .Include("Course")
            //    .Where(x => x.InstructorID == id)
            //    .Select(x => x.Course);

            var courses = (from courseInstrutor in universityContext.CourseInstructors
                           join course in universityContext.Courses
                           on courseInstrutor.CourseID equals course.CourseID
                           where courseInstrutor.InstructorID == id
                           select course);

            return await courses.ToListAsync();
        }
    }
}
