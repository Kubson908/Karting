using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace TorKartingowyCoreMVC.Models
{
    public class RejestrPrac
    {
        [Key]
        public int Id { get; set; }

        public DateTime? Data { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Wpisz wykonane zadanie")]
        public string WykonanaPraca { get; set; } = string.Empty;

        //Navigation Properties
        [Required]
        [ForeignKey("Pracownik")]
        public int PracownikId { get; set; }
        public Pracownik? Pracownik { get; set; } = null;
    }
}
