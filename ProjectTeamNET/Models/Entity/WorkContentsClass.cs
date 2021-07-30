using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectTeamNET.Models.Entity
{
    [Table("m_work_contents_class", Schema = "public")]
    public class WorkContentsClass
    {
        [Key]
        [Required]
        [StringLength(2)]
        [Display(Name = "��Ɠ��e�敪")]
        [Column("work_contents_class")]
        public string Work_contents_class { get; set; }

        [StringLength(40)]
        [Display(Name = "��Ɠ��e�敪��")]
        [Column("work_contents_class_name")]
        public string Work_contents_class_name { get; set; }

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