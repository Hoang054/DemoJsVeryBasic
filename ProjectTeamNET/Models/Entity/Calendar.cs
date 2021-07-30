using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTeamNET.Models.Entity
{
    [Table("m_calendar", Schema = "public")]
    public class Calendar
    {
        [Key]
        [Required]
        [Column("date")]
        public DateTime Date { get; set; }

        [Key]
        [Required]
        [StringLength(8)]
        [Column("site_code")]
        public string Site_code { get; set; }

        [Column("horiday")]
        public bool? Horiday { get; set; }

    }
}
