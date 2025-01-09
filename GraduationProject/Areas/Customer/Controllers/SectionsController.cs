using DataAccess.Repository;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using Utility;

namespace GraduationProject.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = $"{SD.AdminRole} , {SD.Asseitant},{SD.Student},{SD.Professor}")]

    public class SectionsController : Controller
    {
        ISectionsRepository _sectionsRepository;
        ICourseRepository _courseRepository;
        public SectionsController(ISectionsRepository sectionsRepository,ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
            _sectionsRepository = sectionsRepository;
        }

        public IActionResult Index(int page = 1, string search = null)
        {

            int pageSize = 5;
            var totalProducts = _sectionsRepository.GetAll([]).Count();

            //var totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
            var totalPages = Math.Max(1, (int)Math.Ceiling((double)totalProducts / pageSize));


            if (page <= 0) page = 1;
            if (page > totalPages) page = totalPages;
            IQueryable<Sections> sections = _sectionsRepository.GetAll([e => e.Course]);


            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;


            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim();
                sections = sections.Where(e => e.Name.Contains(search));

                if (!sections.Any())
                {
                    ViewBag.ErrorMessage = "No Courses found with that Name.";
                }
            }

            sections = sections.Skip((page - 1) * pageSize).Take(pageSize);

            return View(model: sections.ToList());
        }
        public IActionResult Index2(int CourseID, int page = 1, string search = null)
        {

            int pageSize = 5;
            var totalProducts = _sectionsRepository.GetAll([]).Count();

            //var totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
            var totalPages = Math.Max(1, (int)Math.Ceiling((double)totalProducts / pageSize));


            if (page <= 0) page = 1;
            if (page > totalPages) page = totalPages;
            IQueryable<Sections> sections = _sectionsRepository.GetAll([e => e.Course], expression: e => e.Course.CourseId == CourseID);

            var course = _courseRepository.GetOne(expression: e => e.CourseId == CourseID).FirstOrDefault();
            ViewBag.AssID = course.AssistantId;
            ViewBag.ProfID = course.MemberId;
            
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;


            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim();
                sections = sections.Where(e => e.Name.Contains(search));

                if (!sections.Any())
                {
                    ViewBag.ErrorMessage = "No Courses found with that Name.";
                }
            }

            sections = sections.Skip((page - 1) * pageSize).Take(pageSize);

            return View(model: sections.ToList());
        }

        public IActionResult Create(int CourseID = 0)
        {
            var Course = _courseRepository.GetAll().ToList().Select(e => new SelectListItem { Text = e.Name, Value = e.CourseId.ToString() });
            ViewBag.Course = Course;
            var section = new Sections();
            if (CourseID > 0) { section.CourseId = CourseID; }
            return View(section);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Sections sections, IFormFile SecURL)
        {
            if (ModelState.IsValid)
            {
                if (SecURL.Length > 0) // 99656
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(SecURL.FileName); // .png
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\SectionDocument", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        SecURL.CopyTo(stream);
                    }

                    using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        File(fileStream, "application/pdf");
                    }
                    //File(fileStream, "application/pdf", fileName);
                    sections.SecURL = fileName;
                }
                _sectionsRepository.Add(sections);
                _sectionsRepository.Commit();
                TempData["message"] = "The section is created sucesfully";
                //return RedirectToAction(nameof(Index));
                return RedirectToAction(nameof(Index2), new { CourseID = sections.CourseId });

                //return RedirectToAction(nameof(StudentSections), new { CourseId = sections.CourseId });
            }
            var Course = _courseRepository.GetAll().ToList().Select(e => new SelectListItem { Text = e.Name, Value = e.CourseId.ToString() });
            ViewBag.Course = Course;
            return View(sections);
        }

        public IActionResult Delete(int id)
        {
            var sections = _sectionsRepository.GetOne(expression: e => e.SectionsId == id).FirstOrDefault();
            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\SectionDocument", sections.SecURL);
            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
            }
            _sectionsRepository.Delete(sections);
            _sectionsRepository.Commit();
            TempData["message"] = "The section is deleted sucesfully";
            return RedirectToAction(nameof(Index2), new
            {
                CourseID = sections.CourseId
            });
        }

        public IActionResult StudentSections(int CourseId)
        {
                var Studentsections = _sectionsRepository.GetAll(expression: s => s.CourseId == CourseId);
                return View(Studentsections);
        }
    }
}
