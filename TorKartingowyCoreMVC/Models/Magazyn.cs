using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TorKartingowyCoreMVC.Models
{
    public class Magazyn
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Podaj nazwę")]
        public string Nazwa { get; set; }

        [Required(ErrorMessage = "Wybierz kategorię")]

        public string Kategoria { get; set; }

        [Required(ErrorMessage = "Podaj opis")]
        public string Opis { get; set; }

        [Required(ErrorMessage = "Podaj stan magazynowy")]
        [Range(1, int.MaxValue, ErrorMessage = "Podaj poprawną ilość")]
        public int StanMagazynowy { get; set; }

        public int Wypozyczono { get; set; } = 0;
    }
}
