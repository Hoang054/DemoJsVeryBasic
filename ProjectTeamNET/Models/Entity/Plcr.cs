using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectTeamNET.Models.Entity
{
    [Table("m_plcr", Schema = "public")]
    public class Plcr
    {
        [Key]
        [Required]
        [StringLength(2)]
        [Display(Name = "PLCR�敪�R�[�h")]
        [Column("plcr_code")]
        public string Plcr_code { get; set; }

        [StringLength(40)]
        [Display(Name = "PLCR�敪��")]
        [Column("plcr_name")]
        public string Plcr_name { get; set; }

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