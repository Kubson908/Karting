using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TorKartingowyCoreMVC.Models
{
    public class Harmonogram
    {
        [Key]
        [Required(ErrorMessage = "Podaj od kiedy obowiązuje harmonogram")]
        public string OdKiedy { get; set; }

        [Required(ErrorMessage = "Dodaj plik z harmonogramem")]
        public byte[] ListaGodzin { get; set; }
    }
}
