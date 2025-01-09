using DataAccess.Repository;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;

namespace GraduationProject.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class AssignmentController : Controller
    {
        private readonly ICourseRepository courseRepository;
        IAssignmentRepository AssignmentRepository;

        public AssignmentController(ICourseRepository courseRepository,IAssignmentRepository AssignmentRepository)
        {
            this.courseRepository = courseRepository;
            this.AssignmentRepository = AssignmentRepository;

        }

        public IActionResult Index(int courseId)
        {
            var assignments = AssignmentRepository.GetAll(expression: e => e.CourseId == courseId).ToList();

            var assignment = AssignmentRepository.GetOne(includeProp: [e => e.Course] , expression: e => e.CourseId == courseId).FirstOrDefault();

            if (assignment != null)
            {
                var memberid = assignment.Course.MemberId;
                ViewBag.MemberId = memberid;

            }

            ViewBag.CourseId = courseId;
            var course = courseRepository.GetOne(expression: e => e.CourseId == courseId).FirstOrDefault();
            ViewBag.ProfID =course.MemberId;

            return View(model : assignments);
        }

        public IActionResult Create(int courseId)
        {
            Assignment assignment = new Assignment();

            ViewBag.CourseId = courseId;

            return View(model: assignment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Assignment assignment, IFormFile DocumentUrl)
        {
            if (ModelState.IsValid)
            {
                if (DocumentUrl.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(DocumentUrl.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\CourseAssignment", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        DocumentUrl.CopyTo(stream);
                    }

                    using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        File(fileStream, "application/pdf");
                    }
                    //File(fileStream, "application/pdf", fileName);
                    assignment.DocumentUrl = fileName;
                }
                
                AssignmentRepository.Add(assignment);
                AssignmentRepository.Commit();
                // TempData["message"] = "The lecture is added sucsesfully";
                return RedirectToAction(nameof(Index), new { assignment.CourseId });
            }
            return View(model: assignment);
        }

        public IActionResult Delete(int AssignmentId)
        {
            var assignment = AssignmentRepository.GetOne(expression: e => e.AssignmentId == AssignmentId).FirstOrDefault();
            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\CourseAssignment", assignment.DocumentUrl);
            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
            }
            AssignmentRepository.Delete(assignment);
            AssignmentRepository.Commit();
            //TempData["message"] = "The lecture is deleted sucessfully";
            return RedirectToAction(nameof(Index), new { assignment.CourseId });
        }
    }
}
