using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Student
    {
        public string StudentId { get; set; } = null!;
        [Required]
        [DataType(DataType.Text)]
        [MaxLength(20)]

        public string FName { get; set; } = null!;
        [Required]
        [MaxLength(14)]
        public string SSN { get; set; } = null!;
        [Required]
        [DataType(DataType.Text)]
        [MaxLength(40)]
        public string MName { get; set; } = null!;
        [Required]
        [DataType(DataType.Text)]
        [MaxLength(20)]
        public string LName { get; set; } = null!;
        
        [Required]
        [Column(TypeName = "nvarchar(max)")]

        public EnumGender Gender { get; set; }
        [ValidateNever]
        public string ImgUrl { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string Address { get; set; } = null!;
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
        [Required]

        public EnumLevel Level { get; set; }
        [Required]
        public string Nationailty { get; set; } = null!;
        [Required]

        public DateTime BirthDate { get; set; }
    
        
        public List<StudentCourse> StudentCourses { get; set; }

        public List<StudentPhone> StudentPhones { get; set; }

        public List<StudentAssignment> StudentAssignments { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }



    }
}
