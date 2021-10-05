using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodEv2._0.Models
{
    public class SurveyAnswersViewModel
    {
        [Required]
        [Range(1, 4, ErrorMessage = "Please Answer question 1")]
        public Int16 Answer1 { get; set; }
        [Required]
        [Range(1, 4, ErrorMessage = "Please Answer question 2")]
        public Int16 Answer2 { get; set; }
        [Required]
        [Range(1, 4, ErrorMessage = "Please Answer question 3")]
        public Int16 Answer3 { get; set; }
        [Required]
        [Range(1, 4, ErrorMessage = "Please Answer question 4")]
        public Int16 Answer4 { get; set; }
        [Required]
        [Range(1, 4, ErrorMessage = "Please Answer question 5")]
        public Int16 Answer5 { get; set; }
        [Required]
        [Range(1, 4, ErrorMessage = "Please Answer question 6")]
        public Int16 Answer6 { get; set; }
        [Required]
        [Range(1, 4, ErrorMessage = "Please Answer question 7")]
        public Int16 Answer7 { get; set; }
        [Required]
        [Range(1, 4, ErrorMessage = "Please Answer question 8")]
        public Int16 Answer8 { get; set; }
        [Required]
        [Range(1, 4, ErrorMessage = "Please Answer question 9")]
        public Int16 Answer9 { get; set; }
        [Required]
        [Range(1, 4, ErrorMessage = "Please Answer question 10")]
        public Int16 Answer10 { get; set; }
    }
}
