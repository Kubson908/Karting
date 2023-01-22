using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace TorKartingowyCoreMVC.Models
{
    public class Serwis
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Podaj usterkę")]
        public string Usterka { get; set; } = string.Empty;
        public string? Notatka { get; set; } = "";

        [DisplayName("Data utworzenia")]
        public DateTime? DataUtworzenia { get; set; } = DateTime.Now;

        public bool Wykonano { get; set; } = false;

        //Navigation Properties
        [Required(ErrorMessage = "Podaj numer gokarta"), DisplayName("Numer gokarta")]
        [ForeignKey("Gokart")]
        public int GokartNumer { get; set; }
        public virtual Gokart? Gokart { get; set; }

        [ForeignKey("Pracownik")]
        public int? InstruktorId { get; set; } = null;
        public virtual Pracownik? Instruktor { get; set; } = null;

        [ForeignKey("Pracownik")]
        public int? MechanikId { get; set; }
        public virtual Pracownik? Mechanik { get; set; } = null;
    }
}
