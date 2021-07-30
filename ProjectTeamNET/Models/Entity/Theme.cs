using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectTeamNET.Models.Entity
{
    [Table("m_theme", Schema = "public")]
    public class Theme
    {
        [Key]
        [Required]
        [StringLength(12)]
        [Display(Name = "�e�[�}NO")]
        [Column("theme_no")]
        public string Theme_no { get; set; }

        [StringLength(1)]
        [Display(Name = "���O�̋敪�R�[�h")]
        [Column("internal_external_sales_code")]
        public string Internal_external_sales_code { get; set; }

        [StringLength(1)]
        [Display(Name = "�󒍐�敪�R�[�h")]
        [Column("customer_code")]
        public string Customer_code { get; set; }

        [StringLength(1)]
        [Display(Name = "�󒍓��e�敪�R�[�h")]
        [Column("order_contents_code")]
        public string Order_contents_code { get; set; }

        [StringLength(2)]
        [Display(Name = "�o�^�N")]
        [Column("regist_year")]
        public string Regist_year { get; set; }

        [StringLength(60)]
        [Display(Name = "�e�[�}��1�i�����j")]
        [Column("theme_name1")]
        public string Theme_name1 { get; set; }

        [StringLength(40)]
        [Display(Name = "�e�[�}��2�i�����j")]
        [Column("theme_name2")]
        public string Theme_name2 { get; set; }

        [StringLength(2)]
        [Display(Name = "��Ɠ��e�敪")]
        [Column("work_contents_class")]
        public string Work_contents_class { get; set; }

        [Display(Name = "��t��")]
        [Column("accept_date")]
        public DateTime? Accept_date { get; set; }

        [Display(Name = "�˗���")]
        [Column("request_date")]
        public DateTime? Request_date { get; set; }

        [Display(Name = "�������")]
        [Column("sales_date")]
        public DateTime? Sales_date { get; set; }

        [StringLength(2)]
        [Display(Name = "��v����R�[�h")]
        [Column("accounting_group_code")]
        public string Accounting_group_code { get; set; }

        [StringLength(6)]
        [Display(Name = "����ϗL���N��")]
        [Column("sales_valid_yymm")]
        public string Sales_valid_yymm { get; set; }

        [StringLength(3)]
        [Display(Name = "����ȖڃR�[�h")]
        [Column("sales_object_code")]
        public string Sales_object_code { get; set; }

        [Display(Name = "�󒍋��z")]
        [Column("order_amount")]
        public int? Order_amount { get; set; }

        [Display(Name = "�v�挴����")]
        [Column("plan_cost_rate")]
        public decimal? Plan_cost_rate { get; set; }

        [StringLength(10)]
        [Display(Name = "��ЃR�[�h")]
        [Column("company_code")]
        public string Company_code { get; set; }

        [StringLength(2)]
        [Display(Name = "�o�k�b�q�敪")]
        [Column("plcr_code")]
        public string Plcr_code { get; set; }

        [StringLength(2)]
        [Display(Name = "���㌎�敪")]
        [Column("sales_month_code")]
        public string Sales_month_code { get; set; }

        [StringLength(40)]
        [Display(Name = "���㌎�敪����")]
        [Column("sales_month_code_memo")]
        public string Sales_month_code_memo { get; set; }

        [Display(Name = "�d�|�t���O")]
        [Column("processing_flg")]
        public bool? Processing_flg { get; set; }

        [Display(Name = "����σt���O")]
        [Column("sold_flg")]
        public bool? Sold_flg { get; set; }

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