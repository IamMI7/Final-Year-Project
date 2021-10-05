using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FoodEDAL.Entities
{
    public class Room
    {
        [Key]
        public int RoomId { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 4)]
        public String RoomName { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 4)]
        public String RoomLeader { get; set; }
        [Required]
        [StringLength(6)]
        public String RoomCode { get; set; }
    }
}
