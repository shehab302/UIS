using Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Utility;

namespace DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentPhone> StudentPhones { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<MemberPhone> MemberPhones { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Lectures> Lectures { get; set; }
        public DbSet<Sections> Sections { get; set; }
        public DbSet<Timetable> Timetables { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<StudentAssignment> StudentAssignments { get; set; }
        public DbSet<Inquiry> Inquirys { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public ApplicationDbContext()
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().HasData(
            new IdentityRole { Name = SD.AdminRole, NormalizedName = SD.AdminRole.ToUpper() },
            new IdentityRole { Name = SD.Student, NormalizedName = SD.Student.ToUpper() },
            new IdentityRole { Name = SD.Professor, NormalizedName = SD.Professor.ToUpper() },
            new IdentityRole { Name = SD.Asseitant, NormalizedName = SD.Asseitant.ToUpper() },
            new IdentityRole { Name = SD.Empolyee, NormalizedName = SD.Empolyee.ToUpper() }
        );
            // تخصيص الجداول الأساسية لـ ASP.NET Identity
            builder.Entity<IdentityRole>().ToTable("IdentityRole", "Security");
            builder.Entity<IdentityUser>().ToTable("IdentityUser", "Security");
            builder.Entity<IdentityUserRole<string>>().ToTable("IdentityUserRole", "Security");
            builder.Entity<IdentityUserClaim<string>>().ToTable("IdentityUserClaim", "Security");
            builder.Entity<IdentityUserLogin<string>>().ToTable("IdentityUserLogin", "Security");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("IdentityRoleClaim", "Security");
            builder.Entity<IdentityUserToken<string>>().ToTable("IdentityUserToken", "Security");

            // العلاقة بين الجداول
            builder.Entity<Course>()
                .HasOne(c => c.Member)
                .WithMany(m => m.Courses)
                .HasForeignKey(c => c.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<StudentCourse>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.StudentCourses)
                .HasForeignKey(sc => sc.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<StudentCourse>()
                .HasOne(sc => sc.Course)
                .WithMany(c => c.StudentCourses)
                .HasForeignKey(sc => sc.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<StudentAssignment>()
               .HasOne(sa => sa.Student)
               .WithMany(s => s.StudentAssignments)
               .HasForeignKey(sa => sa.StudentId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<StudentAssignment>()
              .HasOne(sa => sa.Assignment)
              .WithMany(a => a.StudentAssignments)
              .HasForeignKey(sa => sa.AssignmentId)
              .OnDelete(DeleteBehavior.Restrict);


           

            // إعداد القيم التلقائية (auto-increment) بشكل صحيح
            builder.Entity<StudentPhone>()
                .Property(sp => sp.StudentPhoneId)
                .ValueGeneratedOnAdd();

            builder.Entity<MemberPhone>()
                .Property(mp => mp.MemberPhoneId)
                .ValueGeneratedOnAdd();
        }
    }
}
