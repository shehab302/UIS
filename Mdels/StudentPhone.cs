using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class StudentPhone
    {
        public int StudentPhoneId { get; set; }

        public string StudentId { get; set; } = null!;
        public string Phone { get; set; }
        public Student Student { get; set; }
                    
    }
}
