using Models;
using Microsoft.AspNetCore.Identity;
using Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Utility;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;

namespace GraduationProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize (Roles =$"{SD.AdminRole}")]
    public class RegisterationController : Controller
    {
        private readonly IMemberPhoneRepository memberPhoneRepository;
        private readonly IStudentPhoneRepository studentPhoneRepository;
        private readonly IStudentRepository studentRepository;
        private readonly IMemberRepository memberRepository;
        private readonly IEmployeeRepository employeeRepository;
        private readonly IDepartmentRepository departmentRepository;
        private readonly UserManager<IdentityUser> usermanger;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<IdentityUser> signinmanager;

        public RegisterationController(IMemberPhoneRepository memberPhoneRepository,IStudentPhoneRepository studentPhoneRepository,IStudentRepository studentRepository,IMemberRepository memberRepository,IEmployeeRepository employeeRepository ,IDepartmentRepository departmentRepository, UserManager<IdentityUser> usermanger, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signinmanager)
        {
            this.memberPhoneRepository = memberPhoneRepository;
            this.studentPhoneRepository = studentPhoneRepository;
            this.studentRepository = studentRepository;
            this.memberRepository = memberRepository;
            this.employeeRepository = employeeRepository;
            this.departmentRepository = departmentRepository;
            this.usermanger = usermanger;
            this.roleManager = roleManager;
            this.signinmanager = signinmanager;
        }


        public async Task<IActionResult> AddAdmin()
        {
            if (roleManager.Roles.IsNullOrEmpty())
            {
                await roleManager.CreateAsync(new(SD.AdminRole));
                await roleManager.CreateAsync(new(SD.Professor));
                await roleManager.CreateAsync(new(SD.Asseitant));
                await roleManager.CreateAsync(new(SD.Empolyee));
                await roleManager.CreateAsync(new(SD.Student));

            }

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAdmin(AdminVM admin)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = new()
                {
                    UserName = admin.Email,
                    Email = admin.Email,
                    RoleName="Admin"

                };
                var result = await usermanger.CreateAsync(applicationUser, admin.Password);
                if (result.Succeeded)
                {
                    await usermanger.AddToRoleAsync(applicationUser, SD.AdminRole);
                    TempData["message"] = "The admin is added sucsesufuly ";
                    return View();
                }
                else
                    ModelState.AddModelError("Email", "Dublicated Email....");

            }
            return View(admin);
        }
        public IActionResult AddStudent()
        {

            ViewBag.EnumGender = (EnumGender[])Enum.GetValues(typeof(EnumGender));
            ViewBag.EnumLevel = (EnumLevel[])Enum.GetValues(typeof(EnumLevel));
            var department = departmentRepository.GetAll().ToList().Select(e => new SelectListItem { Text = e.Name, Value = e.DepartmentId.ToString() });
            ViewBag.department = department;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStudent(StudentVM student , IFormFile ImgUrl)
        {
            if (ModelState.IsValid)
            {
                if (ImgUrl.Length > 0) 
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImgUrl.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Img", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        ImgUrl.CopyTo(stream);
                    }

                    student.ImgUrl = fileName;
                }
                ApplicationUser applicationUser = new()
                {
                    //UserName = student.FName + "_" + student.MName + "_" + student.LName,
                    UserName=student.Email,
                    Email = student.Email,
                    Address = student.Address,
                    RoleName = "Student"

                };
                var result = await usermanger.CreateAsync(applicationUser, student.SSN);

                if (result.Succeeded)
                {
                    await usermanger.AddToRoleAsync(applicationUser, SD.Student);

                    Student studentinstance = new()
                    {
                        StudentId = applicationUser.Id,
                        SSN = student.SSN,
                        FName = student.FName,
                        MName = student.MName,
                        LName = student.LName,
                        Email = student.Email,
                        Address = student.Address,
                        Gender = student.Gender,
                        BirthDate = student.BirthDate,
                        Level = student.Level,
                        Nationailty = student.Nationailty,
                        ImgUrl = student.ImgUrl,
                        DepartmentId = student.DepartmentId

                    };
                    studentRepository.Add(studentinstance);
                    studentRepository.Commit();
                    if (student.studentPhones != null && student.studentPhones.Any())
                    {
                        var studentPhones = student.studentPhones
                            .Where(p => !string.IsNullOrWhiteSpace(p))
                            .Select(p => new StudentPhone { StudentId = applicationUser.Id, Phone = p })
                        .ToList();

                        studentPhoneRepository.AddRange(studentPhones);
                        studentPhoneRepository.Commit();
                    }
                    TempData["message"] = "The Student is added sucsesufuly ";
                    ViewBag.EnumGender = (EnumGender[])Enum.GetValues(typeof(EnumGender));
                    ViewBag.EnumLevel = (EnumLevel[])Enum.GetValues(typeof(EnumLevel));
                    
                    ViewBag.department = departmentRepository.GetAll().ToList().Select(e => new SelectListItem { Text = e.Name, Value = e.DepartmentId.ToString() });
                    return View();

                }
                else
                {
                    ModelState.AddModelError("FName", "this user name is already exist ");
                }

            }
            ViewBag.EnumGender = (EnumGender[])Enum.GetValues(typeof(EnumGender));
            ViewBag.EnumLevel = (EnumLevel[])Enum.GetValues(typeof(EnumLevel));
            var department = departmentRepository.GetAll().ToList().Select(e => new SelectListItem { Text = e.Name, Value = e.DepartmentId.ToString() });
            ViewBag.department = department;
            return View(student);
        }
        public IActionResult AddProfessor()
        {

            ViewBag.EnumGender = (EnumGender[])Enum.GetValues(typeof(EnumGender));
            var department = departmentRepository.GetAll().ToList().Select(e => new SelectListItem { Text = e.Name, Value = e.DepartmentId.ToString() });
            ViewBag.department = department; 


            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProfessor(MemberVM member, IFormFile ImgUrl)
        {
            if (ModelState.IsValid)
            {
                if (ImgUrl.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImgUrl.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Img", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        ImgUrl.CopyTo(stream);
                    }

                    member.ImgUrl = fileName;
                }
                ApplicationUser applicationUser = new()
                {
                    //UserName = member.FName + "_" + member.MName + "_" + member.LName,
                    UserName = member.Email,
                    Email = member.Email,
                    Address = member.Address,
                    RoleName = "Professor"


                };
                var result = await usermanger.CreateAsync(applicationUser, member.SSN);

                if (result.Succeeded)
                {
                    await usermanger.AddToRoleAsync(applicationUser, SD.Professor);

                    Member ProfMemberInstance = new()
                    {
                        MemberId = applicationUser.Id,
                        SSN = member.SSN,
                        FName = member.FName,
                        MName = member.MName,
                        LName = member.LName,
                        Email = member.Email,
                        Address = member.Address,
                        Gender = member.Gender,
                        BirthDate = member.BirthDate,
                        Nationailty = member.Nationailty,
                        Experence=member.Experence,
                        ImgUrl=member.ImgUrl,
                        DepartmentId = member.DepartmentId,
                        IsProfessor = member.IsProfessor

                    };
                    memberRepository.Add(ProfMemberInstance);
                    memberRepository.Commit();
                    if (member.MemberPhones != null && member.MemberPhones.Any())
                    {
                        var memberPhones = member.MemberPhones
                            .Where(p => !string.IsNullOrWhiteSpace(p))
                            .Select(p => new MemberPhone { MemberId = applicationUser.Id, Phone = p })
                        .ToList();

                        memberPhoneRepository.AddRange(memberPhones);
                        memberPhoneRepository.Commit();
                    }
                    TempData["message"] = "The Professor is added sucsesufuly ";
                    ViewBag.EnumGender = (EnumGender[])Enum.GetValues(typeof(EnumGender));
                    ViewBag.EnumLevel = (EnumLevel[])Enum.GetValues(typeof(EnumLevel));
                    ViewBag.department = ViewBag.department = departmentRepository.GetAll().ToList().Select(e => new SelectListItem { Text = e.Name, Value = e.DepartmentId.ToString() });

                    return View();

                }
                else
                {
                    ModelState.AddModelError("FName", "this user name is already exist ");
                }

            }
            ViewBag.EnumGender = (EnumGender[])Enum.GetValues(typeof(EnumGender));
            var department = departmentRepository.GetAll().ToList().Select(e => new SelectListItem { Text = e.Name, Value = e.DepartmentId.ToString() });
            ViewBag.department = department;
            return View(member);
        }
        
        
        public IActionResult AddAssistant()
        {

            ViewBag.EnumGender = (EnumGender[])Enum.GetValues(typeof(EnumGender));
            var department = departmentRepository.GetAll().ToList().Select(e => new SelectListItem { Text = e.Name, Value = e.DepartmentId.ToString() });
            ViewBag.department = department;


            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAssistant(MemberVM member , IFormFile ImgUrl)
        {
            if (ModelState.IsValid)
            {
                if (ImgUrl.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImgUrl.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Img", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        ImgUrl.CopyTo(stream);
                    }

                    member.ImgUrl = fileName;
                }
                ApplicationUser applicationUser = new()
                {
                    //UserName = member.FName + "_" + member.MName + "_" + member.LName,
                    UserName = member.Email,
                    Email = member.Email,
                    Address = member.Address,
                    RoleName = "Assistant"

                };
                var result = await usermanger.CreateAsync(applicationUser, member.SSN);

                if (result.Succeeded)
                {
                    await usermanger.AddToRoleAsync(applicationUser, SD.Asseitant);

                    Member ProfMemberInstance = new()
                    {
                        MemberId = applicationUser.Id,
                        SSN = member.SSN,
                        FName = member.FName,
                        MName = member.MName,
                        LName = member.LName,
                        Email = member.Email,
                        Address = member.Address,
                        Gender = member.Gender,
                        BirthDate = member.BirthDate,
                        Nationailty = member.Nationailty,
                        Experence=member.Experence,
                        ImgUrl=member.ImgUrl,
                        DepartmentId = member.DepartmentId,
                        IsProfessor = member.IsProfessor

                    };
                    memberRepository.Add(ProfMemberInstance);
                    memberRepository.Commit();
                    if (member.MemberPhones != null && member.MemberPhones.Any())
                    {
                        var memberPhones = member.MemberPhones
                            .Where(p => !string.IsNullOrWhiteSpace(p))
                            .Select(p => new MemberPhone { MemberId = applicationUser.Id, Phone = p })
                        .ToList();

                        memberPhoneRepository.AddRange(memberPhones);
                        memberPhoneRepository.Commit();
                    }
                    TempData["message"] = "The Assistant is added sucsesufuly ";
                    ViewBag.EnumGender = (EnumGender[])Enum.GetValues(typeof(EnumGender));
                    ViewBag.department = ViewBag.department = departmentRepository.GetAll().ToList().Select(e => new SelectListItem { Text = e.Name, Value = e.DepartmentId.ToString() });

                    return View();

                }
                else
                {
                    ModelState.AddModelError("FName", "this user name is already exist ");
                }

            }
            ViewBag.EnumGender = (EnumGender[])Enum.GetValues(typeof(EnumGender));
            var department = departmentRepository.GetAll().ToList().Select(e => new SelectListItem { Text = e.Name, Value = e.DepartmentId.ToString() });
            ViewBag.department = department;
            return View(member);
        }

        public IActionResult AddEmpolyee()
        {

            ViewBag.EnumGender = (EnumGender[])Enum.GetValues(typeof(EnumGender));


            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEmpolyee(EmolyeeVM employee, IFormFile ImgUrl)
        {
            if (ModelState.IsValid)
            {
                if (ImgUrl.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImgUrl.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Img", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        ImgUrl.CopyTo(stream);
                    }

                    employee.ImgUrl = fileName;
                }
                ApplicationUser applicationUser = new()
                {
                    //UserName = employee.FName + "_" + employee.MName + "_" + employee.LName,
                    UserName = employee.Email,
                    Email = employee.Email,
                    Address = employee.Address,
                    RoleName = "Employee"

                };
                var result = await usermanger.CreateAsync(applicationUser, employee.SSN);

                if (result.Succeeded)
                {
                    await usermanger.AddToRoleAsync(applicationUser, SD.Empolyee);

                    Employee EmpInstance = new()
                    {
                        EmployeeId = applicationUser.Id,
                        SSN = employee.SSN,
                        FName = employee.FName,
                        MName = employee.MName,
                        LName = employee.LName,
                        Email = employee.Email,
                        Address = employee.Address,
                        Gender = employee.Gender,
                        BirthDate = employee.BirthDate,
                        Nationailty = employee.Nationailty,
                        ImgUrl=employee.ImgUrl,
                        PhoneNumer = employee.PhoneNumer,

                    };
                    employeeRepository.Add(EmpInstance);
                    employeeRepository.Commit();
                    TempData["message"] = "The Empolyee is added sucsesufuly ";
                    ViewBag.EnumGender = (EnumGender[])Enum.GetValues(typeof(EnumGender));

                    return View();

                }
                else
                {
                    ModelState.AddModelError("FName", "this user name is already exist ");
                }

            }
            ViewBag.EnumGender = (EnumGender[])Enum.GetValues(typeof(EnumGender));
            return View(employee);
        }
    }


}
