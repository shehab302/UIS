using DataAccess.Repository;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using NuGet.Protocol.Core.Types;
using Utility;
using static System.Collections.Specialized.BitVector32;

namespace GraduationProject.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = $"{SD.AdminRole},{SD.Professor},{SD.Student}")]

    public class LecturesController : Controller
    {
        ILecturesRepository _lecturesRepository;
        ICourseRepository _courseRepository;
        public LecturesController(ILecturesRepository lecturesRepository, ICourseRepository courseRepository)
        {

            _lecturesRepository = lecturesRepository;
            _courseRepository = courseRepository;
        }

       
        public IActionResult Index(int page = 1, string search = null)
        {

            int pageSize = 5;
            var totalProducts = _lecturesRepository.GetAll([]).Count();

            //var totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
            var totalPages = Math.Max(1, (int)Math.Ceiling((double)totalProducts / pageSize));


            if (page <= 0) page = 1;
            if (page > totalPages) page = totalPages;
            IQueryable<Lectures> lectures = _lecturesRepository.GetAll([e => e.Course]);


            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;


            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim();
                lectures = lectures.Where(e => e.Name.Contains(search));

                if (!lectures.Any())
                {
                    ViewBag.ErrorMessage = "No Courses found with that Name.";
                }
            }

            lectures = lectures.Skip((page - 1) * pageSize).Take(pageSize);

            return View(model: lectures.ToList());
        }
        public IActionResult Index2(int CourseID, int page = 1, string search = null)
        {

            int pageSize = 5;
            var totalProducts = _lecturesRepository.GetAll([]).Count();

            var totalPages = Math.Max(1, (int)Math.Ceiling((double)totalProducts / pageSize));


            if (page <= 0) page = 1;
            if (page > totalPages) page = totalPages;
            IQueryable<Lectures> lectures = _lecturesRepository.GetAll([e => e.Course],expression:e=>e.Course.CourseId==CourseID);
            var course = _courseRepository.GetOne(expression: e => e.CourseId == CourseID).FirstOrDefault();
            ViewBag.ProfID = course.MemberId;

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;


            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim();
                lectures = lectures.Where(e => e.Name.Contains(search));

                if (!lectures.Any())
                {
                    ViewBag.ErrorMessage = "No Courses found with that Name.";
                }
            }

            lectures = lectures.Skip((page - 1) * pageSize).Take(pageSize);

            return View(model: lectures.ToList());
        }

        public IActionResult Create(int CourseID=0)
        {

            var Course = _courseRepository.GetAll().ToList().Select(e => new SelectListItem { Text = e.Name, Value = e.CourseId.ToString() });
            ViewBag.Course = Course;
            var lecture = new Lectures();
            if (CourseID > 0) {lecture.CourseId= CourseID; }
            return View(lecture);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create(Lectures lecture, IFormFile LecURL)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (LecURL.Length > 0) // 99656
        //        {
        //            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(LecURL.FileName); // .png
        //            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\LectureDocument", fileName);

        //            using (var stream = System.IO.File.Create(filePath))
        //            {
        //                LecURL.CopyTo(stream);
        //            }

        //            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        //            {
        //                File(fileStream, "application/pdf");
        //            }
        //            //File(fileStream, "application/pdf", fileName);
        //            lecture.LecURL = fileName;
        //        }
        //        _lecturesRepository.Add(lecture);
        //        _lecturesRepository.Commit();
        //        TempData["message"] = "The lecture is added sucsesfully";
        //        //return RedirectToAction(nameof(Index));
        //        return RedirectToAction(nameof(Index2), new { CourseID =lecture.CourseId});
        //    }
        //    var Course = _courseRepository.GetAll().ToList().Select(e => new SelectListItem { Text = e.Name, Value = e.CourseId.ToString() });
        //    ViewBag.Course = Course;
        //    return View(lecture);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Lectures lecture, IFormFile LecURL)
        {
            if (ModelState.IsValid)
            {
                // Check if the CourseId exists in the Courses table
                var courseExists = _courseRepository.GetAll().Any(c => c.CourseId == lecture.CourseId);
                if (!courseExists)
                {
                    ModelState.AddModelError("CourseId", "Invalid Course ID. Please select a valid course.");
                    var Course = _courseRepository.GetAll().ToList().Select(e => new SelectListItem { Text = e.Name, Value = e.CourseId.ToString() });
                    ViewBag.Course = Course;
                    return View(lecture);
                }

                // Proceed with file upload and saving the lecture
                if (LecURL != null && LecURL.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(LecURL.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\LectureDocument", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        LecURL.CopyTo(stream);
                    }

                    lecture.LecURL = fileName;
                }
                else
                {
                    Console.WriteLine("No file uploaded or file is empty.");
                }

                try
                {
                    _lecturesRepository.Add(lecture);
                    Console.WriteLine("Lecture added to repository.");

                    _lecturesRepository.Commit();
                    Console.WriteLine("Changes committed to the database.");

                    TempData["message"] = "The lecture is added successfully";
                    return RedirectToAction(nameof(Index2), new { CourseID = lecture.CourseId });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving lecture: {ex.Message}");
                    ModelState.AddModelError("", "An error occurred while saving the lecture. Please try again.");
                    var CourseList = _courseRepository.GetAll().ToList().Select(e => new SelectListItem { Text = e.Name, Value = e.CourseId.ToString() });
                    ViewBag.Course = CourseList;
                    return View(lecture);
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                foreach (var error in errors)
                {
                    Console.WriteLine(error);
                }

                var CourseList = _courseRepository.GetAll().ToList().Select(e => new SelectListItem { Text = e.Name, Value = e.CourseId.ToString() });
                ViewBag.Course = CourseList;
                return View(lecture);
            }
        }


        public IActionResult Delete(int id)
        {
            var lectures = _lecturesRepository.GetOne(expression: e => e.LecturesId == id).FirstOrDefault();
            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\LectureDocument", lectures.LecURL);
            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
            }
            _lecturesRepository.Delete(lectures);
            _lecturesRepository.Commit();
            TempData["message"] = "The lecture is deleted sucessfully";
            return RedirectToAction(nameof(Index2), new
            {
                CourseID=lectures.CourseId
            });
        }


        public IActionResult StudentLectures(int CourseId)
        {
                var Studentlectures = _lecturesRepository.GetAll(expression: s => s.CourseId == CourseId);
                
                return View(Studentlectures);
        }
        


    }
}
