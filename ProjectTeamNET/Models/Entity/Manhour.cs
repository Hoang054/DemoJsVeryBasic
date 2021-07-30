using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectTeamNET.Models.Entity
{
    [Table("t_manhour", Schema = "public")]
    public class Manhour
    {
        [Key]
        [Required]
        [Display(Name = "�N")]
        [Column("year",Order = 0)]
        public Int16 Year { get; set; }

      
        [Required]
        [Display(Name = "��")]
        [Column("month",Order =1)]
        public Int16 Month { get; set; }

      
        [Required]
        [StringLength(9)]
        [Display(Name = "���[�U�[NO")]
        [Column("user_no",Order =2)]
        public string User_no { get; set; }

        [StringLength(10)]
        [Display(Name = "�����R�[�h")]
        [Column("group_code")]
        public string Group_code { get; set; }

        [StringLength(8)]
        [Display(Name = "�T�C�g�R�[�h")]
        [Column("site_code")]
        public string Site_code { get; set; }

        [Required]
        [StringLength(12)]
        [Display(Name = "�e�[�}NO")]
        [Column("theme_no",Order =3)]
        public string Theme_no { get; set; }

        [Required]
        [StringLength(2)]
        [Display(Name = "��Ɠ��e�敪")]
        [Column("work_contents_class",Order =4)]
        public string Work_contents_class { get; set; }

        [Required]
        [StringLength(2)]
        [Display(Name = "��Ɠ��e�R�[�h")]
        [Column("work_contents_code",Order =5)]
        public string Work_contents_code { get; set; }

        [Required]
        [StringLength(2)]
        [Display(Name = "��Ɠ��e�ڍ�")]
        [Column("work_contents_detail",Order =6)]
        public string Work_contents_detail { get; set; }

        [Display(Name = "�s���~�߃t���O")]
        [Column("pin_flg")]
        public bool? Pin_flg { get; set; }

        [Display(Name = "���v")]
        [Column("total")]
        public double Total { get; set; }

        [Display(Name = "1��")]
        [Column("day1")]
        public double Day1 { get; set; }

        [Display(Name = "2��")]
        [Column("day2")]
        public double Day2 { get; set; }

        [Display(Name = "3��")]
        [Column("day3")]
        public double Day3 { get; set; }

        [Display(Name = "4��")]
        [Column("day4")]
        public double Day4 { get; set; }

        [Display(Name = "5��")]
        [Column("day5")]
        public double Day5 { get; set; }

        [Display(Name = "6��")]
        [Column("day6")]
        public double Day6 { get; set; }

        [Display(Name = "7��")]
        [Column("day7")]
        public double Day7 { get; set; }

        [Display(Name = "8��")]
        [Column("day8")]
        public Double Day8 { get; set; }

        [Display(Name = "9��")]
        [Column("day9")]
        public Double Day9 { get; set; }

        [Display(Name = "10��")]
        [Column("day10")]
        public Double Day10 { get; set; }

        [Display(Name = "11��")]
        [Column("day11")]
        public Double Day11 { get; set; }

        [Display(Name = "12��")]
        [Column("day12")]
        public Double Day12 { get; set; }

        [Display(Name = "13��")]
        [Column("day13")]
        public Double Day13 { get; set; }

        [Display(Name = "14��")]
        [Column("day14")]
        public Double Day14 { get; set; }

        [Display(Name = "15��")]
        [Column("day15")]
        public Double Day15 { get; set; }

        [Display(Name = "16��")]
        [Column("day16")]
        public Double Day16 { get; set; }

        [Display(Name = "17��")]
        [Column("day17")]
        public Double Day17 { get; set; }

        [Display(Name = "18��")]
        [Column("day18")]
        public Double Day18 { get; set; }

        [Display(Name = "19��")]
        [Column("day19")]
        public Double Day19 { get; set; }

        [Display(Name = "20��")]
        [Column("day20")]
        public Double Day20 { get; set; }

        [Display(Name = "21��")]
        [Column("day21")]
        public Double Day21 { get; set; }

        [Display(Name = "22��")]
        [Column("day22")]
        public Double Day22 { get; set; }

        [Display(Name = "23��")]
        [Column("day23")]
        public Double Day23 { get; set; }

        [Display(Name = "24��")]
        [Column("day24")]
        public Double Day24 { get; set; }

        [Display(Name = "25��")]
        [Column("day25")]
        public Double  Day25 { get; set; }

        [Display(Name = "26��")]
        [Column("day26")]
        public Double Day26 { get; set; }

        [Display(Name = "27��")]
        [Column("day27")]
        public Double Day27 { get; set; }

        [Display(Name = "28��")]
        [Column("day28")]
        public Double Day28 { get; set; }

        [Display(Name = "29��")]
        [Column("day29")]
        public Double Day29 { get; set; }

        [Display(Name = "30��")]
        [Column("day30")]
        public Double Day30 { get; set; }

        [Display(Name = "31��")]
        [Column("day31")]
        public Double  Day31 { get; set; }

        [StringLength(8)]
        [Display(Name = "�m����t")]
        [Column("fix_date")]
        public string Fix_date { get; set; }
     

    }
}