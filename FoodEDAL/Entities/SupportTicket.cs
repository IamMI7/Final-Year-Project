using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FoodEDAL.Entities
{
    public class SupportTicket
    {
        [Key]
        public Int32 Id { get; set; }
        [Required]
        [StringLength(20)]
        public String Name { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }
        [Required]
        [StringLength(30)]
        public String Subject { get; set; }
        [Required]
        [StringLength(500)]
        public String Message { get; set; }
        [StringLength(10)]
        public String Status { get; set; }
    }
}
