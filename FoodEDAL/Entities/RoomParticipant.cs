using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FoodEDAL.Entities
{
    public class RoomParticipant
    {
        [Key]
        public Int32 Id { get; set; }
        [Required]
        [StringLength(6)]
        public String RoomCode { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 4)]
        public String Participant { get; set; }
    }
}
