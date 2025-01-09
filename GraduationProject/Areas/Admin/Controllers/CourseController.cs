using DataAccess.Repository;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models;
using Utility;

namespace GraduationProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{SD.AdminRole}")]

    public class CourseController : Controller
    {
        ICourseRepository CourseRepository;

        IDepartmentRepository DepartmentRepository;

        IMemberRepository MemberRepository;
        public CourseController(ICourseRepository CourseRepository, IDepartmentRepository DepartmentRepository, IMemberRepository memberRepository)
        {
            this.CourseRepository = CourseRepository;

            this.DepartmentRepository = DepartmentRepository;

            this.MemberRepository = memberRepository;

        }
        public IActionResult Index(int page = 1, string search = null)
        {

            int pageSize = 5;
            var totalCourses = CourseRepository.GetAll([]).Count();

            //var totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
            var totalPages = Math.Max(1, (int)Math.Ceiling((double)totalCourses / pageSize));


            if (page <= 0) page = 1;
            if (page > totalPages) page = totalPages;
            IQueryable<Course> courses = CourseRepository.GetAll([e => e.Department, e => e.Member]);


            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;


            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim();
                courses = courses.Where(e => e.Name.Contains(search));

                if (!courses.Any())
                {
                    ViewBag.ErrorMessage = "No Courses found with that Name.";
                }
            }

            courses = courses.Skip((page - 1) * pageSize).Take(pageSize);
            

            return View(model: courses.ToList());
        }

        public IActionResult Create()
        {
            Course course = new Course();

            ViewBag.departments = DepartmentRepository.GetAll().ToList().Select(e => new SelectListItem { Text = e.Name, Value = e.DepartmentId.ToString() });

            ViewBag.members = MemberRepository.GetAll(expression:e=>e.IsProfessor==1).ToList().Select(e => new SelectListItem { Text = e.FName + " " + e.MName + " " + e.LName, Value = e.MemberId.ToString() });

            ViewBag.members2 = MemberRepository.GetAll(expression: e => e.IsProfessor != 1).ToList().Select(e => new SelectListItem { Text = e.FName + " " + e.MName + " " + e.LName, Value = e.MemberId.ToString() });


            return View(model: course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Course course, IFormFile DocumentUrl)
        {
            course.Degree = course.AttendanceDegree + course.PracticalDegree + course.FinalDegree + course.MidTermDegree + course.OralDegree;

            if (ModelState.IsValid)
            {
                if (DocumentUrl.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(DocumentUrl.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\CourseDocument", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        DocumentUrl.CopyTo(stream);
                    }

                    using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        File(fileStream, "application/pdf");
                    }
                    //File(fileStream, "application/pdf", fileName);
                    course.DocumentUrl = fileName;
                }
                CourseRepository.Add(course);
                CourseRepository.Commit();
                // TempData["message"] = "The lecture is added sucsesfully";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.departments = DepartmentRepository.GetAll().ToList().Select(e => new SelectListItem { Text = e.Name, Value = e.DepartmentId.ToString() });

            ViewBag.members = MemberRepository.GetAll(expression: e => e.IsProfessor == 1).ToList().Select(e => new SelectListItem { Text = e.FName + " " + e.MName + " " + e.LName, Value = e.MemberId.ToString() });

            ViewBag.members2 = MemberRepository.GetAll(expression: e => e.IsProfessor != 1).ToList().Select(e => new SelectListItem { Text = e.FName + " " + e.MName + " " + e.LName, Value = e.MemberId.ToString() });


            return View(model: course);
        }
        public IActionResult Delete(int courseId)
        {
            var course = CourseRepository.GetOne(expression: e => e.CourseId == courseId).FirstOrDefault();
            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\CourseDocument", course.DocumentUrl);
            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
            }
            CourseRepository.Delete(course);
            CourseRepository.Commit();
            //TempData["message"] = "The lecture is deleted sucessfully";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int courseId)
        {
            var course = CourseRepository.GetOne(expression: e => e.CourseId == courseId).FirstOrDefault();

            ViewBag.departments = DepartmentRepository.GetAll().ToList().Select(e => new SelectListItem { Text = e.Name, Value = e.DepartmentId.ToString() });

            ViewBag.members = MemberRepository.GetAll(expression: e => e.IsProfessor == 1).ToList().Select(e => new SelectListItem { Text = e.FName + " " + e.MName + " " + e.LName, Value = e.MemberId.ToString() });

            ViewBag.members2 = MemberRepository.GetAll(expression: e => e.IsProfessor != 1).ToList().Select(e => new SelectListItem { Text = e.FName + " " + e.MName + " " + e.LName, Value = e.MemberId.ToString() });


            return View(course);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Course course, IFormFile DocumentUrl)
        {
            course.Degree = course.AttendanceDegree + course.PracticalDegree + course.FinalDegree + course.MidTermDegree + course.OralDegree;
            if (ModelState.IsValid)
            {
                var oldcourse = CourseRepository.GetOne(expression: e => e.CourseId == course.CourseId, tracked: false).FirstOrDefault();

                if (DocumentUrl != null && DocumentUrl.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(DocumentUrl.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\CourseDocument", fileName);
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\CourseDocument", oldcourse.DocumentUrl);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        DocumentUrl.CopyTo(stream);
                    }

                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }

                    using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        File(fileStream, "application/pdf");
                    }
                    //File(fileStream, "application/pdf", fileName);
                    course.DocumentUrl = fileName;
                }

                else
                {
                    course.DocumentUrl = oldcourse.DocumentUrl;
                }
                CourseRepository.Edit(course);
                CourseRepository.Commit();
                // TempData["message"] = "The lecture is added sucsesfully";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.departments = DepartmentRepository.GetAll().ToList().Select(e => new SelectListItem { Text = e.Name, Value = e.DepartmentId.ToString() });

            ViewBag.members = MemberRepository.GetAll(expression: e => e.IsProfessor == 1).ToList().Select(e => new SelectListItem { Text = e.FName + " " + e.MName + " " + e.LName, Value = e.MemberId.ToString() });

            ViewBag.members2 = MemberRepository.GetAll(expression: e => e.IsProfessor != 1).ToList().Select(e => new SelectListItem { Text = e.FName + " " + e.MName + " " + e.LName, Value = e.MemberId.ToString() });


            return View(model: course);
        }

    }
}
