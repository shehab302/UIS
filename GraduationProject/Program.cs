using DataAccess;
using Microsoft.EntityFrameworkCore;

using System;
using Microsoft.AspNetCore.Identity;
using DataAccess.Repository;
using DataAccess.Repository.IRepository;
using Models;
using Utility;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace GraduationProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            builder.Services.AddDbContext<ApplicationDbContext>(
               options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
               );

            builder.Services.AddScoped<IStudentRepository, StudentRepository>();
            builder.Services.AddScoped<IStudentPhoneRepository, StudentPhoneRepository>();
            builder.Services.AddScoped<IStudentCourseRepository, StudentCourseRepository>();
            builder.Services.AddScoped<ITimetableRepository, TimetableRepository>();
            builder.Services.AddScoped<ITableLecSecRepositry, TableLecSecRepositry>();
            builder.Services.AddScoped<ISectionRepository, SectionRepository>();
            builder.Services.AddScoped<ILectureRepository, LectureRepository>();
            builder.Services.AddScoped<IInquiryRepositry, InquiryRepositry>();

            builder.Services.AddTransient<IEmailSender, EmailSender>();


            builder.Services.AddScoped<IMemberRepository, MemberRepository>();
            builder.Services.AddScoped<IMemberPhoneRepository, MemberPhoneRepository>();

            builder.Services.AddScoped<ICourseRepository, CourseRepository>();

            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();

            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            builder.Services.AddScoped<ILecturesRepository, LecturesRepository>();

            builder.Services.AddScoped<ISectionsRepository, SectionsRepository>();

            builder.Services.AddScoped<IAssignmentRepository, AssignmentRepository>();

            builder.Services.AddScoped<IStudentAssignmentRepository, StudentAssignmentRepository>();

            builder.Services.AddIdentity<IdentityUser,IdentityRole>
                (options => {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequiredLength = 8;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;

                    options.SignIn.RequireConfirmedEmail = false; 
                    options.SignIn.RequireConfirmedPhoneNumber = false;
                    options.SignIn.RequireConfirmedAccount=false;

                     options.Lockout.AllowedForNewUsers = false;
                }
                ).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            builder.Services.AddScoped<EmailService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.MapRazorPages();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }

        

    }
}
