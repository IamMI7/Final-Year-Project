using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FoodEDAL.Entities
{
    public class Tip
    {
        [Key]
        public Int32 Id { get; set; }
        [Required]
        [StringLength(200)]
        public string TipTitle { get; set; }
        [Required]
        [StringLength(500)]
        public string Tips { get; set; }
    }
}
