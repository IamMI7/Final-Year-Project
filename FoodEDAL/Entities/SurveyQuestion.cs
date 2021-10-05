using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FoodEDAL.Entities
{
    public class SurveyQuestion
    {
        [Key]
        public Int32 Id { get; set; }
        [Required]
        [StringLength(500)]
        public String Question { get; set; }
        [StringLength(100)]
        [Required]
        public String Answer1 { get; set; }
        [StringLength(100)]
        [Required]
        public String Answer2 { get; set; }
        [StringLength(100)]
        [Required]
        public String Answer3 { get; set; }
        [StringLength(100)]
        [Required]
        public String Answer4 { get; set; }
    }
}
