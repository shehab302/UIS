using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Models
{
    public class Employee
    {
        public string EmployeeId { get; set; } = null!;
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
        [ValidateNever]
        public string ImgUrl { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(max)")]

        public EnumGender Gender { get; set; }
        [Required]

        public string Address { get; set; } = null!;
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
        [Required]

        [DataType(DataType.Text)]

        public string Nationailty { get; set; } = null!;
        [Required]

        public DateTime BirthDate { get; set; }
      
        public string PhoneNumer { get; set; } = null!;

       
    }
}
