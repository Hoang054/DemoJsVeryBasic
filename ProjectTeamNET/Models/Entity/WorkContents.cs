using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectTeamNET.Models.Entity
{
    [Table("m_work_contents", Schema = "public")]
    public class WorkContents
    {
        [Key]
        [Required]
        [StringLength(2)]
        [Display(Name = "��Ɠ��e�敪")]
        [Column("work_contents_class")]
        public string Work_contents_class { get; set; }


        [Required]
        [StringLength(2)]
        [Display(Name = "��Ɠ��e�R�[�h")]
        [Column("work_contents_code")]
        public string Work_contents_code { get; set; }

        [StringLength(50)]
        [Display(Name = "��Ɠ��e��")]
        [Column("work_contents_code_name")]
        public string Work_contents_code_name { get; set; }

        [StringLength(60)]
        [Display(Name = "����")]
        [Column("memo")]
        public string Memo { get; set; }

        [Required]
        [StringLength(2)]
        [Display(Name = "���v�P�ʃR�[�h")]
        [Column("subtotal_code")]
        public string Subtotal_code { get; set; }

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