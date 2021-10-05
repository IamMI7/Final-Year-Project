using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodEv2._0.Models
{
    public class RoomViewModel
    {
        public int RoomId { get; set; }
        [Required]
        [StringLength(15, ErrorMessage = "Room Name Must be within 4 to 15 characters in length", MinimumLength = 4)]
        public String RoomName { get; set; }
        public String RoomLeader { get; set; }

        [Required(ErrorMessage = "Please enter a 6 digit code")]
        [MaxLength(6, ErrorMessage = "Please enter a valid 6 digit room code")]
        [MinLength(6, ErrorMessage = "Please enter a valid 6 digit room code.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Please enter a valid 6 digit room code.")]
        public String RoomCode { get; set; }
    }
}
