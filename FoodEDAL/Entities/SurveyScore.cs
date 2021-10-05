using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FoodEDAL.Entities
{
    public class SurveyScore
    {
        [Key]
        [StringLength(15, MinimumLength = 4)]
        public String Username { get; set; }
        [Required]
        public Int16 Score { get; set; }
    }
}
