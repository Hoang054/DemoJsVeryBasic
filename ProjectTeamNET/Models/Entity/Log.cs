using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectTeamNET.Models.Entity
{
    [Table("t_log", Schema = "public")]
    public class Log
    {
        [Key]
        [Required]
        [Display(Name = "����")]
        [Column("date_time")]
        public DateTime Date_time { get; set; }

        [Required]
        [StringLength(9)]
        [Display(Name = "���[�U�[NO")]
        [Column("user_no")]
        public string User_no { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "���j���[ID")]
        [Column("screen_id")]
        public string Screen_id { get; set; }

        [Required]
        [StringLength(8)]
        [Display(Name = "�A�N�V�����^�C�v")]
        [Column("action_type")]
        public string Action_type { get; set; }

        [Required]
        [StringLength(7)]
        [Display(Name = "����")]
        [Column("result")]
        public string Result { get; set; }

        [StringLength(256)]
        [Display(Name = "���1")]
        [Column("info1")]
        public string Info1 { get; set; }

        [StringLength(256)]
        [Display(Name = "���2")]
        [Column("info2")]
        public string Info2 { get; set; }

    }
}