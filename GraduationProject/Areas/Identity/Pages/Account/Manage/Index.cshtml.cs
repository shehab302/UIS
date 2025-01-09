// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DataAccess.Repository;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Utility;

namespace GraduationProject.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IMemberRepository memberRepository;
        private readonly IStudentRepository studentRepository;
        private readonly IEmployeeRepository employeeRepository;

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IMemberRepository memberRepository,
            IStudentRepository studentRepository,
            IEmployeeRepository employeeRepository


            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.memberRepository = memberRepository;
            this.studentRepository = studentRepository;
            this.employeeRepository = employeeRepository;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ProfileImg { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        /// 
        

        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            //var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            var member = memberRepository.GetOne(expression: e => e.MemberId == user.Id).FirstOrDefault();
            var student = studentRepository.GetOne(expression: e => e.StudentId == user.Id).FirstOrDefault();
            var empolyee = employeeRepository.GetOne(expression: e => e.EmployeeId == user.Id).FirstOrDefault();
            ApplicationUser Admin = new ApplicationUser()
            {
                Id = user.Id,
                RoleName = SD.AdminRole,
                Email = user.Email
            };
           
            if (member != null)
            {
            FullName = $"{member.FName} {member.MName} {member.LName}";
            Email = member.Email;
                ProfileImg = member.ImgUrl;

          

            }
            else if (student != null)
            {
            FullName = $"{student.FName} {student.MName} {student.LName}";
            Email = student.Email;
                ProfileImg = student.ImgUrl;

          

            }
            else if (empolyee != null)
            {
            FullName = $"{empolyee.FName} {empolyee.MName} {empolyee.LName}";
            Email = empolyee.Email;
            ProfileImg = empolyee.ImgUrl;
            }
            else if (Admin != null)
            {
                FullName = "Admin";
                Email = Admin.Email;

            }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            //var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            //if (Input.PhoneNumber != phoneNumber)
            //{
            //    var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
            //    if (!setPhoneResult.Succeeded)
            //    {
            //        StatusMessage = "Unexpected error when trying to set phone number.";
            //        return RedirectToPage();
            //    }
            //}

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
