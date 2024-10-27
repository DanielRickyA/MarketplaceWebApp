using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace MarketWebApp.Models
{
    public class Keranjang
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Produk")]
        public int IdProduk { get; set; }
        [Required]
        [ForeignKey("Customer")]
        public int IdCustomer { get; set; }
        public virtual Produk? Produk { get; set; }
        public virtual Customer? Customer { get; set; }
        [Required]
        public int? Jumlah { get; set; }
        public int? SubTotal { get; set; }

        public string GetFormattedSubTotal()
        {
            return SubTotal.HasValue ? string.Format(new CultureInfo("id-ID"), "Rp {0:N0}", SubTotal) : "Rp 0";
        }

    }
}
