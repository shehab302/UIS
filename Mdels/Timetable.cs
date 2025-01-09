using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Timetable
    {
        public int TimetableId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public EnumTableDay Day { get; set; }   
        public List<TableLecSec> TableLecSecs { get; set; }




    }
}
