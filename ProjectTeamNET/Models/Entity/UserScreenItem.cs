using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectTeamNET.Models.Entity
{
    [Table("m_user_screen_item", Schema = "public")]
    public class UserScreenItem
    {
        [Key]
        [Required]
        [StringLength(26)]
        [Display(Name = "�T���Q�[�g�L�[")]
        [Column("surrogate_key")]
        public string Surrogate_key { get; set; }

        [Required]
        [StringLength(9)]
        [Display(Name = "���[�U�[NO")]
        [Column("user_no")]
        public string User_no { get; set; }

        [Required]
        [StringLength(256)]
        [Display(Name = "���URL")]
        [Column("screen_url")]
        public string Screen_url { get; set; }

        [Required]
        [StringLength(256)]
        [Display(Name = "��ʍ���")]
        [Column("screen_item")]
        public string Screen_item { get; set; }

        [StringLength(256)]
        [Display(Name = "��ʓ��͒l")]
        [Column("screen_input")]
        public string Screen_input { get; set; }

        [StringLength(40)]
        [Display(Name = "�ۑ���")]
        [Column("save_name")]
        public string Save_name { get; set; }

    }
}