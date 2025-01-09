using DataAccess.Repository;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.ViewModels;
using Utility;

namespace GraduationProject__FacuiltySystem__.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{SD.AdminRole},{SD.Asseitant},{SD.Professor},{SD.Empolyee}")]

    public class ManagementController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IStudentRepository studentRepository;
        private readonly IDepartmentRepository departmentRepository;
        private readonly IMemberRepository memberRepository;
        private readonly IEmployeeRepository employeeRepository;
        public ManagementController(UserManager<IdentityUser> userManager, IStudentRepository studentRepository, IDepartmentRepository departmentRepository , IMemberRepository memberRepository , IEmployeeRepository employeeRepository)
        {
            this.userManager = userManager;
            this.studentRepository = studentRepository;
            this.departmentRepository = departmentRepository;
            this.memberRepository = memberRepository;
            this.memberRepository = memberRepository;
            this.employeeRepository = employeeRepository;
        }
        public IActionResult Index(string search = null, int page = 1)
        {
            int pageSize = 5;
            var totalProducts = studentRepository.GetAll([]).Count();
            ;
            var totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

            if (page <= 0) page = 1;
            if (totalPages == 0) totalPages = 1;
            if (page > totalPages) page = totalPages;
            IQueryable<Student> students = studentRepository.GetAll([e => e.Department]);
            ;

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;


 
            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim();
                students = students.Where(e => e.SSN.Contains(search));  

                if (!students.Any())   
                {
                    ViewBag.ErrorMessage = "No students found with that SSN.";   
                }
            }
            students = students.Skip((page - 1) * pageSize).Take(pageSize);

            return View(students.ToList());
        }
        public async Task<IActionResult> Delete(string id)
        {
            var student = studentRepository.GetOne().FirstOrDefault(e => e.StudentId == id);
            if (student != null)
            {
                var newUser = await userManager.FindByIdAsync(id);
                var result = await userManager.DeleteAsync(newUser);
                if (result.Succeeded)
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Img", student.ImgUrl);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                    studentRepository.Delete(student);
                    studentRepository.Commit();
                    TempData["success"] = "Delete student successfuly";
                    return RedirectToAction("Index");
                }
            }
            else

                TempData["error"] = "somthing is wrong... please go to the IT engineer for more detailes";
            return RedirectToAction("Index");
        }

        public IActionResult Index_Professor(int page = 1, string search = null)
        {
 
            int pageSize = 5;
            var totalProducts = memberRepository.GetAll([]).Count();
            ;
            var totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

            if (page <= 0) page = 1;
            if (totalPages == 0) totalPages = 1;
            if (page > totalPages) page = totalPages;
            IQueryable<Member> Professors = memberRepository.GetAll([e => e.Department], e => e.IsProfessor == 1);
            ;

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;

            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim();
                Professors = Professors.Where(e => e.SSN.Contains(search));

                if (!Professors.Any())
                {
                    ViewBag.ErrorMessage = "No Professors found with that SSN.";
                }
            }
            Professors = Professors.Skip((page - 1) * pageSize).Take(pageSize);

            return View(Professors.ToList());

        }
       


        
        public async Task<IActionResult> Delete_Professor(string id)
        {
            var member = memberRepository.GetOne().FirstOrDefault(e => e.MemberId == id);
            if (member != null)
            {
                var newUser = await userManager.FindByIdAsync(id);
                var result = await userManager.DeleteAsync(newUser);
                if (result.Succeeded)
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Img", member.ImgUrl);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                    memberRepository.Delete(member);
                    memberRepository.Commit();
                    TempData["success"] = "Delete Professor successfuly";
                    return RedirectToAction("Index_Professor");
                }
            }
            else

                TempData["error"] = "somthing is wrong... please go to the IT engineer for more detailes";
            return RedirectToAction("Index_Professor");
        }







        public IActionResult Index_Assistant(int page = 1, string search = null)
        {
 

            int pageSize = 5;
            var totalProducts = memberRepository.GetAll([]).Count();
            ;
            var totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

            if (page <= 0) page = 1;
            if (totalPages == 0) totalPages = 1;
            if (page > totalPages) page = totalPages;
            IQueryable<Member> Assistants = memberRepository.GetAll([e => e.Department], e => e.IsProfessor == 0);
            ;

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;
            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim();
                Assistants = Assistants.Where(e => e.SSN.Contains(search));

                if (!Assistants.Any())
                {
                    ViewBag.ErrorMessage = "No Assistants found with that SSN.";
                }
            }
            Assistants = Assistants.Skip((page - 1) * pageSize).Take(pageSize);

            return View(Assistants.ToList());

        }
      
        public async Task<IActionResult> Delete_Assistant(string id)
        {
            var member = memberRepository.GetOne().FirstOrDefault(e => e.MemberId == id);
            if (member != null)
            {
                var newUser = await userManager.FindByIdAsync(id);
                var result = await userManager.DeleteAsync(newUser);
                if (result.Succeeded)
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Img", member.ImgUrl);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                    memberRepository.Delete(member);
                    memberRepository.Commit();
                    TempData["success"] = "Delete Assistant successfuly";
                    return RedirectToAction("Index_Assistant");
                }
            }
            else

                TempData["error"] = "somthing is wrong... please go to the IT engineer for more detailes";
            return RedirectToAction("Index_Assistant");

        }







        public IActionResult Index_Empolyee(int page = 1, string search = null)
        {
 

            int pageSize = 5;
            var totalProducts = memberRepository.GetAll([]).Count();
            ;
            var totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

            if (page <= 0) page = 1;
            if (totalPages == 0) totalPages = 1;
            if (page > totalPages) page = totalPages;
            IQueryable<Employee> employees = employeeRepository.GetAll();
            ;

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;
            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim();
                employees = employees.Where(e => e.SSN.Contains(search));

                if (!employees.Any())
                {
                    ViewBag.ErrorMessage = "No Employees found with that SSN.";
                }
            }
            employees = employees.Skip((page - 1) * pageSize).Take(pageSize);


            return View(employees.ToList());

        }
      
        public async Task<IActionResult> Delete_Empolyee(string id)
        {
            var employee = employeeRepository.GetOne().FirstOrDefault(e => e.EmployeeId == id);
            if (employee != null)
            {
                var newUser = await userManager.FindByIdAsync(id);
                var result = await userManager.DeleteAsync(newUser);
                if (result.Succeeded)
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Img", employee.ImgUrl);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                    employeeRepository.Delete(employee);
                    employeeRepository.Commit();
                    TempData["success"] = "Delete Employee successfuly";
                    return RedirectToAction("Index_Empolyee");
                }
            }
            TempData["error"] = "somthing is wrong... please go to the IT engineer for more detailes";
            return RedirectToAction("Index_Empolyee");

        }


    }
}
