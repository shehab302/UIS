using Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Utility;
using Models.ViewModels;
using DataAccess.Repository;

namespace GraduationProject.Areas.Customer.Controllers
{
    [Area("Customer")]

    public class HomeController : Controller
    {
        private readonly IInquiryRepositry inquiryRepositry;
        private readonly EmailService emailService;
        private readonly ILogger<HomeController> _logger;
        private readonly IStudentRepository studentRepository;
        private readonly IMemberRepository memberRepository;

        public HomeController(IInquiryRepositry inquiryRepositry,EmailService emailService, ILogger<HomeController> logger , IStudentRepository studentRepository , IMemberRepository memberRepository)
        {
            this.inquiryRepositry = inquiryRepositry;
            this.emailService = emailService;
            _logger = logger;
            this.studentRepository = studentRepository;
            this.memberRepository = memberRepository;

        }

        public IActionResult Index()
        {
             return View();
        }

        public IActionResult About()
        {
           return View();
        }
        public IActionResult Contact()
        {

             return View( );
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(EmailVM emailInstance)
        {
            if (ModelState.IsValid)
            {
                string subject = $"New Question from {emailInstance.Name}";
                string messageBody = $@"
                <p><strong>Name:</strong> {emailInstance.Name}</p>
                <p><strong>Email:</strong> {emailInstance.Email}</p>
                <p><strong>Subject:</strong> {emailInstance.Subject}</p>
                <p><strong>Message:</strong> {emailInstance.Message}</p>";

                await emailService.SendEmailAsync(emailInstance.Email, subject, messageBody);
                var inquiry = new Inquiry
                {
                    StudentEmail = emailInstance.Email,
                    Subject = emailInstance.Subject,
                    Message = emailInstance.Message,
                    DateSent = DateTime.Now,
                    Status = EnumStatus.Unread,
                    EmployeeNote="",
                    EmployeeResponse=""

                };

                inquiryRepositry.Add(inquiry);
                inquiryRepositry.Commit();
                TempData["message"] = "Your message has been sent successfully!";
                return View();
            }

            return View(emailInstance);
        }
    

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
