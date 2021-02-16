using AutoMapper;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
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
    public class StudentsController : Controller
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

        public StudentsController()
        {
            this.mapper = MvcApplication.MapperConfiguration.CreateMapper();
        }

        // GET: Student
        public async Task<ActionResult> Index()
        {
            var studentService = new StudentService(new StudentRepository(UniversityContext));
            var listStudents = await studentService.GetAll();
            var listStudentsDTO = listStudents.Select(x => mapper.Map<StudentDTO>(x));

            return View(listStudentsDTO);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(StudentDTO studentDTO)
        {
            if (ModelState.IsValid)
            {
                var studentService = new StudentService(new StudentRepository(UniversityContext));
                var student = mapper.Map<Student>(studentDTO);
                student = await studentService.Insert(student);

                return RedirectToAction("Index", "Students");
            }

            return View(studentDTO);
        }

        // GET: Students/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var studentService = new StudentService(new StudentRepository(UniversityContext));
            var student = await studentService.GetById(id.Value);

            if (student == null)
            {
                return HttpNotFound();
            }

            var studentDTO = mapper.Map<StudentDTO>(student);

            return View(studentDTO);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var studentService = new StudentService(new StudentRepository(UniversityContext));
            Student student = await studentService.GetById(id.Value);

            if (student == null)
            {
                return HttpNotFound();
            }

            var studentDTO = mapper.Map<StudentDTO>(student);

            return View(studentDTO);
        }

        // POST: Courses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(StudentDTO studentDTO)
        {
            if (ModelState.IsValid)
            {
                var studentService = new StudentService(new StudentRepository(UniversityContext));

                var student = mapper.Map<Student>(studentDTO);
                student = await studentService.Update(student);

                return RedirectToAction("Index", "Courses");
            }

            return View(studentDTO);
        }

        // GET: Students/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var studentService = new StudentService(new StudentRepository(UniversityContext));
            Student student = await studentService.GetById(id.Value);

            if (student == null)
            {
                return HttpNotFound();
            }

            var studentDTO = mapper.Map<StudentDTO>(student);

            return View(studentDTO);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var studentService = new StudentService(new StudentRepository(UniversityContext));

            try
            {
                if (!await studentService.DeleteCheckOnEntity(id))
                    await studentService.Delete(id);
                else
                    throw new Exception("ForeignKeys");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                var student = await studentService.GetById(id);

                var studentDTO = mapper.Map<StudentDTO>(student);

                return View("Delete", studentDTO);
            }

            return RedirectToAction("Index");
        }

    }
}