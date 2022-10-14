using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

#nullable disable

namespace EHSApi.Models
{
    public partial class Buyer
    {
        public Buyer()
        {
            Carts = new HashSet<Cart>();
        }

        public int BuyerId { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public DateTime DateofBirth { get; set; }
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Invalid Mobile Number")]
        public string PhoneNo { get; set; }
        [Required]
        public string EmailId { get; set; }
        [JsonIgnore]
        public virtual ICollection<Cart> Carts { get; set; }
    }
}
