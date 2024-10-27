using System.ComponentModel.DataAnnotations;

namespace MarketWebApp.Models
{
    public class Customer
    {
        public int Id { get; set; }
     
        public string? Name { get; set; }
        [Required]

        public string? Email { get; set; }
        [Required]

        public string? Password { get; set; }
     
        public string? Phone { get; set; }
    }
}
