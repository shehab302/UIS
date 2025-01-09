using DataAccess.Repository;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Utility;

namespace GraduationProject.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = $"{SD.AdminRole} ,{SD.Student}")]

    public class StudentDetailesController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IMemberRepository memberRepository;
        private readonly IStudentRepository studentRepository;
        private readonly ITimetableRepository timetableRepository;
        private readonly ILectureRepository lectureRepository;

        public StudentDetailesController(UserManager<IdentityUser> userManager, IStudentRepository studentRepository, IMemberRepository memberRepository , ITimetableRepository timetableRepository , ILectureRepository lectureRepository)
        {
            this.userManager = userManager;
            this.studentRepository = studentRepository;
            this.memberRepository = memberRepository;
            this.timetableRepository = timetableRepository;
            this.lectureRepository = lectureRepository;
        }

        public IActionResult Index(string StudentID)
        {


            ViewBag.Lectures = lectureRepository.GetAll()
                                       .ToList();

             var student = studentRepository.GetOne([e => e.Department,c=>c.StudentPhones], e => e.StudentId == StudentID).FirstOrDefault();
            ViewBag.professors = memberRepository.GetAll([e => e.Department], e => e.IsProfessor == 1);
            return View(student);
        }
    }
}