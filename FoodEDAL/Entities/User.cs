using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FoodEDAL.Entities
{
    public class User
    {
        //Primary Key
        [Key]
        public String Username { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 2)]
        public String FirstName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 2)]
        public String LastName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 4)]
        public String Password { get; set; }

        [Required]
        public String Email { get; set; }

        [Required]
        public String ContactNo { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }

        //Foreign Keys
        
        public Int32 AuthLevelRefId { get; set; }
    }
}
