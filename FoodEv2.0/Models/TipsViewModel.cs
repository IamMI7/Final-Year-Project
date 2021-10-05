using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodEv2._0.Models
{
    public class TipsViewModel
    {
        [Key]
        public Int32 Id { get; set; }
        [Required]
        [StringLength(200)]
        public string TipTitle { get; set; }
        [Required]
        [StringLength(500)]
        public string Tip { get; set; }
    }
}
