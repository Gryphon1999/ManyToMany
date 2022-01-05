using ManyToManyRelation.DAL;
using ManyToManyRelation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ManyToManyRelation.Controllers
{
    public class StudentCourseController : Controller
    {
        private readonly RelationDbContext db;

        public StudentCourseController(RelationDbContext context )
        {
            db = context;
        }
        // GET: StudentCourseController
        public ActionResult Index()
        {
            var data = db.StudentCourses.Include(s=>s.Student).Include(s=>s.Course).ToList();
            return View(data);
        }

        // GET: StudentCourseController/Details/5
        public ActionResult Details(int id)
        {
            var data = db.StudentCourses.Include(s=>s.Student).Include(s=>s.Course).FirstOrDefault(s=>s.Id==id);
            return View(data);
        }

        // GET: StudentCourseController/Create
        public ActionResult Create()
        {
            ViewBag.StudentId = new SelectList(db.students,"StudentId","Name");
            ViewBag.Course = db.courses;
            return View();
        }

        // POST: StudentCourseController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int StudentId, int[] CourseId)
        {
            try
            {
                foreach (int item in CourseId)
                {
                StudentCourse studentCourse = new StudentCourse();
                    studentCourse.StudentId = StudentId;
                    studentCourse.CourseId = item;
                db.StudentCourses.Add(studentCourse);
                }
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StudentCourseController/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.StudentId = new SelectList(db.students, "StudentId", "Name");
            ViewBag.CourseId = new SelectList(db.courses, "CourseId", "CourseName");
            ViewBag.Course = db.courses;
            var data = db.StudentCourses.Include(s => s.Student).Include(s => s.Course).FirstOrDefault(s => s.Id == id);
            return View(data);
        }

        // POST: StudentCourseController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id,StudentCourse studentCourse)
        {
            try
            {
                var data = db.StudentCourses.Find(id);
                data.StudentId = studentCourse.StudentId;
                data.CourseId = studentCourse.CourseId;
                db.StudentCourses.Update(data);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StudentCourseController/Delete/5
        public ActionResult Delete(int id)
        {
            var data = db.StudentCourses.Include(s => s.Student).Include(s => s.Course).FirstOrDefault(s => s.Id == id);
            return View(data);
        }

        // POST: StudentCourseController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, StudentCourse studentCourse)
        {
            try
            {
                var data = db.StudentCourses.Find(id);
                data.StudentId = studentCourse.StudentId;
                data.CourseId = studentCourse.CourseId;
                db.StudentCourses.Remove(data);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
