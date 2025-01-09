using DataAccess.Repository;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.ViewModels;

namespace GraduationProject.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class StudentAssignmentController : Controller
    {
        IStudentAssignmentRepository StudentAssignmentRepository;

        IAssignmentRepository AssignmentRepository;

        ICourseRepository CourseRepository;

        public StudentAssignmentController(IStudentAssignmentRepository StudentAssignmentRepository , IAssignmentRepository AssignmentRepository , ICourseRepository CourseRepository)
        {
            this.StudentAssignmentRepository = StudentAssignmentRepository;

            this.AssignmentRepository = AssignmentRepository;

            this.CourseRepository = CourseRepository;


        }
        public IActionResult CourseAssignments(int courseId , string studentId)
        {
            var assignments = AssignmentRepository.GetAll(expression: e => e.CourseId == courseId).ToList();

            ViewBag.studentId = studentId;

            ViewBag.courseId = courseId;

            var course = CourseRepository.GetOne(expression: e => e.CourseId == courseId).FirstOrDefault();

            ViewBag.level = course.CourseLevel;

            return View(model: assignments);
        }

        public IActionResult Index(string studentId , int courseId)
        {
            var studentassignments = StudentAssignmentRepository.GetAll(includeProp: [e => e.Assignment], expression: e => e.StudentId == studentId && e.Assignment.CourseId == courseId).ToList();

            ViewBag.studentId = studentId;

            ViewBag.courseId = courseId;

            return View(model: studentassignments);
        }

        public IActionResult Index2(string studentId, int courseId)
        {
            var studentassignments = StudentAssignmentRepository.GetAll(includeProp: [e => e.Assignment], expression: e => e.StudentId == studentId && e.Assignment.CourseId == courseId).ToList();

            ViewBag.courseId = courseId;

            return View(model: studentassignments);
        }

        public IActionResult Create(int assignmentId , string studentId, int courseId)
        {
            StudentAssignment studentassignment = new StudentAssignment();

            ViewBag.studentId = studentId;

            ViewBag.assignmentId = assignmentId;

            ViewBag.courseId = courseId;

            return View(model: studentassignment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(StudentAssignment studentassignment, IFormFile DocumentUrl , int courseId)
        {
            if (ModelState.IsValid)
            {
                if (DocumentUrl.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(DocumentUrl.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\StudentAssignment", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        DocumentUrl.CopyTo(stream);
                    }

                    using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        File(fileStream, "application/pdf");
                    }
                    //File(fileStream, "application/pdf", fileName);
                    studentassignment.DocumentUrl = fileName;
                }

                StudentAssignmentRepository.Add(studentassignment);
                StudentAssignmentRepository.Commit();
                // TempData["message"] = "The lecture is added sucsesfully";
                return RedirectToAction(nameof(CourseAssignments), new { courseId = courseId, studentId = studentassignment.StudentId });
            }

            ViewBag.studentId = studentassignment.StudentId;

            ViewBag.assignmentId = studentassignment.AssignmentId;

            return View(model: studentassignment);
        }

        public IActionResult Delete(int StudentAssignmentId)
        {
            var studentassignment = StudentAssignmentRepository.GetOne(expression: e => e.StudentAssignmentId == StudentAssignmentId).FirstOrDefault();
            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\StudentAssignment", studentassignment.DocumentUrl);
            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
            }
            StudentAssignmentRepository.Delete(studentassignment);
            AssignmentRepository.Commit();
            //TempData["message"] = "The lecture is deleted sucessfully";
            return RedirectToAction(nameof(Index), new { studentassignment.StudentId });
        }
    }
}
