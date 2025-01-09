using DataAccess.Repository;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Utility;

namespace GraduationProject.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles =$"{SD.AdminRole},{SD.Asseitant}")]
    public class AssistantController : Controller
    {
        private readonly ICourseRepository courseRepository;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IMemberRepository memberRepository;

        public AssistantController(ICourseRepository courseRepository,UserManager<IdentityUser> userManager, IMemberRepository memberRepository)
        {
            this.courseRepository = courseRepository;
            this.userManager = userManager;
            this.memberRepository = memberRepository;
        }

        public IActionResult Index(string AssID)
        {
            var Assistant = memberRepository.GetOne(expression: e => e.MemberId == AssID, includeProp: [e => e.Department,c=>c.MemberPhones]).FirstOrDefault();

            ViewBag.age = DateTime.Today.Year - Assistant.BirthDate.Year;
            return View(Assistant);
        }
        public IActionResult AssistantCourses(string AssID)
        {
            var Courses = courseRepository.GetAll(includeProp: [e => e.Department], expression: e => e.AssistantId== AssID).ToList();
            return View(model: Courses);

        }

    }
}
