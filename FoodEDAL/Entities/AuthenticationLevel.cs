using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FoodEDAL.Entities
{
    public class AuthenticationLevel
    {
        [Key]
        public Int32 AuthId { get; set; }

        [Required]
        public String AuthName { get; set; }

        [ForeignKey("AuthLevelRefId")]
        public ICollection<User> Users { get; set; }
    }
}
