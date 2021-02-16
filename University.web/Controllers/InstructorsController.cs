using AutoMapper;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using University.BL.Data;
using University.BL.DTOs;
using University.BL.Models;
using University.BL.Repositories.Implements;
using University.BL.Services.Implements;

namespace University.web.Controllers
{
    //[Authorize]
    public class InstructorsController : Controller
    {
        private UniversityContext _universityContext;
        public UniversityContext UniversityContext
        {
            get
            {
                return _universityContext ?? HttpContext.GetOwinContext().Get<UniversityContext>();
            }
            private set
            {
                _universityContext = value;
            }
        }

        private IMapper mapper;

        public InstructorsController()
        {
            this.mapper = MvcApplication.MapperConfiguration.CreateMapper();
        }

        // GET: Instructors
        public async Task<ActionResult> Index(int? id)
        {
            var InstructorService = new InstructorService(new InstructorRepository(UniversityContext));

            if(id != null)
            {
                var courses = await InstructorService.GetCoursesByInstructor(id.Value);
                ViewBag.Courses = courses.Select(x => mapper.Map<CourseDTO>(x));
            }
                

            var instructors = await InstructorService.GetAll();
            var instructrosDTO = instructors.Select(x => mapper.Map<InstructorDTO>(x));


            return View(instructrosDTO);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(InstructorDTO instructorDTO)
        {
            if (ModelState.IsValid){

                var instructorService = new InstructorService(new InstructorRepository(UniversityContext));
                var instructor = mapper.Map<Instructor>(instructorDTO);
                instructor = await instructorService.Insert(instructor);

                return RedirectToAction("Index", "Instructors");
            }

            return View(instructorDTO);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            var instructorService = new InstructorService(new InstructorRepository(UniversityContext));
            var instructor = await instructorService.GetById(id.Value);
            var instructorDTO = mapper.Map<InstructorDTO>(instructor);

            return View(instructorDTO);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(InstructorDTO instructorDTO)
        {
            if (ModelState.IsValid)
            {

                var instructorService = new InstructorService(new InstructorRepository(UniversityContext));
                var instructor = mapper.Map<Instructor>(instructorDTO);
                instructor = await instructorService.Update(instructor);

                return RedirectToAction("Index", "Instructors");
            }

            return View(instructorDTO);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var instructorService = new InstructorService(new InstructorRepository(UniversityContext));
            Instructor instructor = await instructorService.GetById(id.Value);

            if (instructor == null)
            {
                return HttpNotFound();
            }

            var instructorDTO = mapper.Map<InstructorDTO>(instructor);

            return View(instructorDTO);
        }

        // POST: Instructor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var instructorService = new InstructorService(new InstructorRepository(UniversityContext));

            try
            {
                if (!await instructorService.DeleteCheckOnEntity(id))
                    await instructorService.Delete(id);
                else
                    throw new Exception("ForeignKeys");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                var instructor = await instructorService.GetById(id);

                var instructorDTO = mapper.Map<CourseDTO>(instructor);

                return View("Delete", instructorDTO);
            }

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Details(int? id)
        {
            var instructorService = new InstructorService(new InstructorRepository(UniversityContext));
            var instructor = await instructorService.GetById(id.Value);
            var instructorDTO = mapper.Map<InstructorDTO>(instructor);

            return View(instructorDTO);
        }

    }
}