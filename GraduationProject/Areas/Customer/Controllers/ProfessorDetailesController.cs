using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using Utility;

namespace GraduationProject.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = $"{SD.AdminRole} ,{SD.Professor}")]

    public class ProfessorDetailesController : Controller
    {
        private readonly IStudentRepository studentRepository;
        private readonly IStudentCourseRepository studentcourseRepository;
        private readonly ICourseRepository courseRepository;
        private readonly IMemberRepository memberRepository;

        public ProfessorDetailesController(IStudentRepository studentRepository,IStudentCourseRepository studentcourseRepository,ICourseRepository courseRepository, IMemberRepository memberRepository)
        {
            this.studentRepository = studentRepository;
            this.studentcourseRepository = studentcourseRepository;
            this.courseRepository = courseRepository;
            this.memberRepository = memberRepository;
        }

        public IActionResult Index(string ProfID)
        {
            //var professor=await userManager.FindByIdAsync(ProfID);
           var professor=memberRepository.GetOne(expression:e => e.MemberId == ProfID, includeProp: [e => e.Department,c=>c.MemberPhones]).FirstOrDefault();

            ViewBag.age = DateTime.Today.Year - professor.BirthDate.Year;
            return View(professor);
        }
        public IActionResult ProfessorCourses(string ProfID)
        {
            var Courses = courseRepository.GetAll(includeProp: [e=>e.Department], expression:e=>e.Member.MemberId==ProfID).ToList();
            return View(model: Courses);

        }

        public IActionResult StudentCourses(int CourseID)
        {
            var course = courseRepository.GetOne(expression: e => e.CourseId == CourseID).FirstOrDefault();
            var students = studentcourseRepository.GetAll(expression: e => e.CourseId == CourseID, includeProp: [s => s.Student, sc => sc.Student.Department]).Select(s => s.Student).ToList();
            ViewBag.CourseName = course.Name;
            ViewBag.CourseId = CourseID;
            ViewBag.ProfID = course.MemberId;



            return View(students);
        }

    }
}
