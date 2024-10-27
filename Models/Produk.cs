using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace MarketWebApp.Models
{
    public class Produk
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        [Required]
        public String? Name { get; set; }
        [Required]
        public int? Stock { get; set; }
        [Required]
        public int? Price { get; set; }
        public String? Image { get; set; }
        [Required]
        public String? Description { get; set; }
        public string GetFormattedPrice()
        {
            return Price.HasValue ? string.Format(new CultureInfo("id-ID"), "Rp {0:N0}", Price) : "Rp 0";
        }
    }
}
