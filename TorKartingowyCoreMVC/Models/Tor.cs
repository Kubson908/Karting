using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TorKartingowyCoreMVC.Models
{
    public class Tor
    {
        [Key, DisplayName("Numer toru")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Podaj rodzaj toru")]
        public string Rodzaj { get; set; }

        [Required(ErrorMessage = "Podaj maksymalne obłożenie"), DisplayName("Maksymalne obłożenie")]
        public int Pojemnosc { get; set; }

        //Navigation Properties
        public List<Rezerwacja> Rezerwacja { get; set; } = new List<Rezerwacja>();
    }
}
