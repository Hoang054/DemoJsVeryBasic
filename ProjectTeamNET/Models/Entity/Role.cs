using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectTeamNET.Models.Entity
{
    [Table("m_role", Schema = "public")]
    public class Role
    {
        [Key]
        [Required]
        [StringLength(8)]
        [Display(Name = "�������[���R�[�h")]
        [Column("role_code")]
        public string Role_code { get; set; }

        [StringLength(40)]
        [Display(Name = "�������[����")]
        [Column("role_name")]
        public string Role_name { get; set; }

    
        [Required]
        [StringLength(30)]
        [Display(Name = "���URL")]
        [Column("screen_url")]
        public string Screen_url { get; set; }

        [Required]
        [StringLength(1)]
        [Display(Name = "�@��敪")]
        [Column("function_class")]
        public string Function_class { get; set; }

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