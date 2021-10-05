using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodEv2._0.Models
{
    public class ParticipantViewModel
    {
        public Int32 Id { get; set; }
        [Required]
        [StringLength(6)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Please enter a valid 6 digit room code.")]
        public String RoomCode { get; set; }
        public String Participant { get; set; }
    }
}
