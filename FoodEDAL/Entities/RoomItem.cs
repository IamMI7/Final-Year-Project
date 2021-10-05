using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FoodEDAL.Entities
{
    public class RoomItem
    {
        [Key]
        public Int32 Id { get; set; }
        [Required]
        [StringLength(20)]
        public String ItemType { get; set; }
        [Required]
        [StringLength(30)]
        public String ItemName { get; set; }
        [Required]
        public Decimal ItemQuantity { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime ItemExpiry { get; set; }
        [Required]
        [StringLength(6)]
        public String RoomCode { get; set; }
    }
}
