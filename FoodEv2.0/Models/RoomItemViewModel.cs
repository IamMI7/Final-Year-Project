using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodEv2._0.Models
{
    public class RoomItemViewModel
    {
        public Int32 Id { get; set; }
        [Required]
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

        public List<SelectListItem> ItemTypeList { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "Meat", Text = "Meat" },
            new SelectListItem { Value = "Vegetable", Text = "Vegetable" },
            new SelectListItem { Value = "Seafood", Text = "Seafood" },
            new SelectListItem { Value = "Fungi", Text = "Fungi" },
            new SelectListItem { Value = "Bread", Text = "Bread" },
            new SelectListItem { Value = "Sauce", Text = "Sauce" },
            new SelectListItem { Value = "Powder", Text = "Powdered Ingredient" },
            new SelectListItem { Value = "Herb", Text = "Herb" },
            new SelectListItem { Value = "Spice", Text = "Spice" },
            new SelectListItem { Value = "Other", Text = "Other" }
        };
    }
}
