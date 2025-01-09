using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class MemberPhone
    {
        public int MemberPhoneId { get; set; } // Primary key for StudentPhone

        public string MemberId { get; set; }
        public string Phone { get; set; }
        public Member Member { get; set; }
    }
}
