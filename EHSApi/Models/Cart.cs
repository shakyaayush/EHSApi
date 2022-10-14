using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

#nullable disable

namespace EHSApi.Models
{
    public partial class Cart
    {
        public int CartId { get; set; }
        [Required]
        public int? BuyerId { get; set; }
        [Required]
        public int? PropertyId { get; set; }
        [JsonIgnore]
        public virtual Buyer Buyer { get; set; }
        [JsonIgnore]
        public virtual Property Property { get; set; }
    }
}
