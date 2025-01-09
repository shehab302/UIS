using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Assignment
    {
        public int AssignmentId { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [ValidateNever]
        public string DocumentUrl { get; set; } = null!;


        public int CourseId { get; set; }

        [ValidateNever]
        public Course Course { get; set; }

        [ValidateNever]
        public List<StudentAssignment> StudentAssignments { get; set; }


    }
}
