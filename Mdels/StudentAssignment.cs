using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class StudentAssignment
    {
        public int StudentAssignmentId { get; set; } // Primary key for StudentAssignment

        public int AssignmentId { get; set; }

        public string StudentId { get; set; }

        [ValidateNever]
        public string DocumentUrl { get; set; } = null!;

        [ValidateNever]
        public Student Student { get; set; }

        [ValidateNever]
        public Assignment Assignment { get; set; }
    }
}
