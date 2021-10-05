using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodEv2._0.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "This Field must be Filled")]
        [StringLength(15, ErrorMessage = "Username must be between 4 and 15 in length", MinimumLength = 4)]
        public String Username { get; set; }

        [Required(ErrorMessage = "This Field must be Filled")]
        [StringLength(20, ErrorMessage = "First Name must be between 2 and 20 in length", MinimumLength = 2)]
        public String FirstName { get; set; }

        [Required(ErrorMessage = "This Field must be Filled")]
        [StringLength(20, ErrorMessage = "Last Name must be between 2 and 20 in length", MinimumLength = 2)]
        public String LastName { get; set; }

        [Required(ErrorMessage = "This Field must be Filled")]
        [DataType(DataType.Password)]
        [StringLength(15, ErrorMessage = "Password must be between 4 and 15 in length", MinimumLength = 4)]
        public String Password { get; set; }

        [Required(ErrorMessage = "This Field must be Filled")]
        [Compare(nameof(Password), ErrorMessage = "Password and Confirm Password do not match")]
        public String ConfirmPassword { get; set; }

        [Required(ErrorMessage = "This Field must be Filled")]
        [EmailAddress]
        public String EmailAddress { get; set; }

        [Required(ErrorMessage = "This Field must be Filled")]
        public String ContactNo { get; set; }

        [Required(ErrorMessage = "This Field must be Filled")]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }

        public Int32 AuthLevel { get; set; }
    }
}
