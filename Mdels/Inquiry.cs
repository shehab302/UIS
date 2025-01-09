using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Inquiry
    {
        public int Id { get; set; }
        public string StudentEmail { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime DateSent { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public EnumStatus Status { get; set; } // "Unread" or "Responded"
        public string EmployeeNote { get; set; }
        public string EmployeeResponse { get; set; }  // Employee's response
        public DateTime? ResponseDate { get; set; }  // Date when response was given
    }
    public enum EnumStatus
    {
        Unread,
        Responded
    }
}
