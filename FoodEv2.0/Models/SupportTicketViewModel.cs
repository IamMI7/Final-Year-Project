using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodEv2._0.Models
{
    public class SupportTicketViewModel
    {
        public Int32 Id { get; set; }
        [Required(ErrorMessage = "Please fill in your name")]
        [StringLength(20, ErrorMessage = "Must be a maximum of 20 characters")]
        public String Name { get; set; }
        [Required(ErrorMessage = "Please fill in your Email Address")]
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }
        [Required(ErrorMessage = "Please fill in a subject")]
        [StringLength(30, ErrorMessage = "Must be a maximum of 30 characters")]
        public String Subject { get; set; }
        [Required(ErrorMessage = "Please write down the issue you are facing in detail")]
        [StringLength(500, ErrorMessage = "Must be a maximum of 500 characters")]
        public String Message { get; set; }
        public String Status { get; set; }
    }
}
