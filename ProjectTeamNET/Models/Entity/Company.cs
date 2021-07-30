using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTeamNET.Models.Entity
{
    [Table("m_company", Schema = "public")]
    public class Company
    {
        [Key]
        [Required]
        [StringLength(10)]
        [Column("company_code")]
        public string Company_code { get; set; }

        [StringLength(60)]
        [Column("company_name")]
        public string Company_name { get; set; }

        [StringLength(40)]
        [Column("company_short_name")]
        public string Company_short_name { get; set; }

        [Column("sort_no")]
        public decimal? Sort_no { get; set; }

        [Column("del_flg")]
        public bool? Del_flg { get; set; }

        [Column("create_date")]
        public DateTime? Create_date { get; set; }

        [StringLength(9)]
        [Column("create_user")]
        public string Create_user { get; set; }

        [Column("update_date")]
        public DateTime? Update_date { get; set; }

        [StringLength(9)]
        [Column("update_user")]
        public string Update_user { get; set; }

    }
}
