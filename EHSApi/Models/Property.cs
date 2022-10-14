using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

#nullable disable

namespace EHSApi.Models
{
    public partial class Property
    {
        public Property()
        {
            Carts = new HashSet<Cart>();
        }

        public int PropertyId { get; set; }
        [Required]
        public string PropertyName { get; set; }
        [Required]
        public string PropertyType { get; set; }
        [Required]
        public string PropertyOption { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public decimal PriceRange { get; set; }
        [Required]
        public decimal? InitialDeposit { get; set; }
        [Required]
        public string Landmark { get; set; }
        [Required]
        public string IsActive { get; set; }
        [Required]
        public int? SellerId { get; set; }
        [JsonIgnore]
        public virtual Seller Seller { get; set; }
        [JsonIgnore]
        public virtual ICollection<Cart> Carts { get; set; }
    }
}
