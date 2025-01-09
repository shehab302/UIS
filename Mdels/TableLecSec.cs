using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class TableLecSec
    {
        public int TableLecSecId { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public EnumTypeTable Type { get; set; }
        [Required]
        public string Name { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public int TimetableId { get; set; }

    }
}
