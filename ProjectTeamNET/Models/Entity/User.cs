using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectTeamNET.Models.Entity
{
    [Table("m_user", Schema = "public")]
    public class User
    {
        [Key]
        [Required]
        [StringLength(9)]
        [Display(Name = "���[�U�[NO")]
        [Column("user_no")]
        public string User_no { get; set; }

        [StringLength(20)]
        [Display(Name = "���[�U�[��")]
        [Column("user_name")]
        public string User_name { get; set; }

        [StringLength(10)]
        [Display(Name = "�����R�[�h")]
        [Column("group_code")]
        public string Group_code { get; set; }

        [StringLength(8)]
        [Display(Name = "�T�C�g�R�[�h")]
        [Column("site_code")]
        public string Site_code { get; set; }

        [StringLength(10)]
        [Display(Name = "�������[���R�[�h")]
        [Column("role_code")]
        public string Role_code { get; set; }

        [StringLength(256)]
        [Display(Name = "���[���A�h���X")]
        [Column("mail_address")]
        public string Mail_address { get; set; }

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