using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTeamNET.Models.Entity
{
    [Table("m_group", Schema = "public")]
    public class Group
    {
        [Key]
        [Required]
        [StringLength(10)]
        [Display(Name = "�����R�[�h")]
        [Column("group_code")]
        public string Group_code { get; set; }

        [StringLength(60)]
        [Display(Name = "������")]
        [Column("group_name")]
        public string Group_name { get; set; }

        [StringLength(2)]
        [Display(Name = "��v����R�[�h")]
        [Column("accounting_group_code")]
        public string Accounting_group_code { get; set; }

        [StringLength(60)]
        [Display(Name = "��v���喼")]
        [Column("accounting_group_name")]
        public string Accounting_group_name { get; set; }

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
