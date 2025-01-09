using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Course
    {
        public int CourseId { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        [Required]
        public int Hours { get; set; }
        [ValidateNever]
        public int Degree { get; set; }
        [ValidateNever]
        public string DocumentUrl { get; set; } = null!;
        [Required]
        public EnumLevel CourseLevel { get; set; }

        [Required]
        public string AssistantId { get; set; }

        [ValidateNever]
        public List<Lectures> Lectures { get; set; }

        [ValidateNever]
        public List<Sections> Sections { get; set; }

        [ValidateNever]
        public List<StudentCourse> StudentCourses { get; set; }

        [ValidateNever]
        public Member Member { get; set; }

        [ValidateNever]
        public Department Department { get; set; }
        public string MemberId { get; set; } 
        public int DepartmentId { get; set; }

        [ValidateNever]
        public List<Assignment> Assignments { get; set; }

        [Required]
        public int AttendanceDegree { get; set; }
        [Required]
        public int PracticalDegree { get; set; }
        [Required]
        public int MidTermDegree { get; set; }
        [Required]
        public int OralDegree { get; set; }
        [Required]
        public int FinalDegree { get; set; }


    }
}
