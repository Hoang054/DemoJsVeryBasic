using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTeamNET.Models.Entity
{
    [Table("m_order_contents", Schema = "public")]
    public class OrderContents
    {
        [Key]
        [Required]
        [StringLength(1)]
        [Display(Name = "�󒍓��e�敪�R�[�h")]
        [Column("order_contents_code")]
        public string Order_contents_code { get; set; }

        [StringLength(40)]
        [Display(Name = "�󒍓��e�敪��")]
        [Column("order_contents_name")]
        public string Order_contents_name { get; set; }

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
