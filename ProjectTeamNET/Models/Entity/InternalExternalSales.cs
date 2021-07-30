using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTeamNET.Models.Entity
{
    [Table("m_internal_external_sales", Schema = "public")]
    public class InternalExternalSales
    {      
        [Key]
        [Required]
        [StringLength(1)]
        [Display(Name = "���O�̋敪�R�[�h")]
        [Column("internal_external_sales_code")]
        public string Internal_external_sales_code { get; set; }

        [StringLength(40)]
        [Display(Name = "���O�̋敪��")]
        [Column("internal_external_sales_name")]
        public string Internal_external_sales_name { get; set; }

        [Display(Name = "�����")]
        [Column("sort_no")]
        public decimal? Sort_no { get; set; }

        [Display(Name = "�폜�t���O")]
        [Column("del_flg")]
        public bool? Del_flg { get; set; }

        [Display(Name = "�o�^����")]
        [Column("create_date")]
        public DateTime? Create_date { get; set; }

        [StringLength(9)]
        [Display(Name = "�o�^���[�UID")]
        [Column("create_user")]
        public string Create_user { get; set; }

        [Display(Name = "�X�V����")]
        [Column("update_date")]
        public DateTime? Update_date { get; set; }

        [StringLength(9)]
        [Display(Name = "�X�V���[�UID")]
        [Column("update_user")]
        public string Update_user { get; set; }
    }
}
