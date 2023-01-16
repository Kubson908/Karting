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

        public string? Notatka { get; set; } = "";

        public bool Wykonano { get; set; } = false;

        //Navigation Properties
        [Required]
        [ForeignKey("Gokart")]
        public int GokartNumer { get; set; }
        public virtual Gokart? Gokart { get; set; }

        [Required]
        [ForeignKey("Pracownik")]
        public int InstruktorId { get; set; }
        public virtual Pracownik? Intruktor { get; set; } = null;

        [Required]
        [ForeignKey("Pracownik")]
        public int? MechanikId { get; set; }
        public virtual Pracownik? Mechanik { get; set; } = null;
    }
}
