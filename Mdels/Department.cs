using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]

        public string Description { get; set; } = null!;
        public int NumOfStudent { get; set; }
        public int NumOfCourses { get; set; }
        public ICollection<Student> Students { get; } = new List<Student>();
        public ICollection<Member> members { get; } = new List<Member>();

        [ValidateNever]
        public List<Course> Courses { get; set; }



    }
}
