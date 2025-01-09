using DataAccess.Repository;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utility;

namespace GraduationProject.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = $"{SD.AdminRole},{SD.Empolyee}")]

    public class EmpolyeeDetailesController : Controller
    {
        private readonly IEmployeeRepository employeeRepository;

        public EmpolyeeDetailesController(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        public IActionResult Index(string EmpID)
        {
            //var professor=await userManager.FindByIdAsync(ProfID);
            var empolyee = employeeRepository.GetOne(expression: e => e.EmployeeId == EmpID).FirstOrDefault();

            ViewBag.age = DateTime.Today.Year - empolyee.BirthDate.Year;
            return View(empolyee);
        }
    }
}
