using System.Collections.Generic;
using System.Threading.Tasks;
using University.BL.Models;
using University.BL.Repositories;
using University.BL.Repositories.Implements;

namespace University.BL.Services.Implements
{
    public class CourseService : GenericService<Course>, ICourseService
    {
        private readonly ICourseRepository courseRepository;
       public CourseService(ICourseRepository courseRepository) : base(courseRepository)
       {
            this.courseRepository = courseRepository;
       }

        public async Task<bool> DeleteCheckOnEntity(int id)
        {
            var flag = await courseRepository.DeleteCheckOnEntity(id);
            return flag;
        }

        public async Task<IEnumerable<Student>> GetStudentsByCourses(int id)
        {
            return await courseRepository.GetStudentsByCourses(id);
        }
    }
}
