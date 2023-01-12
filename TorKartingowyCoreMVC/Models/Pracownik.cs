using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TorKartingowyCoreMVC.Models

{
    public class Pracownik
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Podaj e-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Podaj hasło"), DisplayName("Hasło")]
        public string Haslo { get; set; }

        [Required(ErrorMessage = "Podaj imię"), DisplayName("Imię")] 
        public string Imie { get; set; }

        [Required(ErrorMessage = "Podaj nazwisko")]
        public string Nazwisko { get; set; }

        [Required(ErrorMessage = "Podaj stanowisko")]
        public string Stanowisko { get; set; }

        [Required(ErrorMessage = "Podaj numer telefonu")]
        [StringLength(9)]
        [MinLength(9, ErrorMessage = "Błędny numer telefonu")]
        public string Telefon { get; set; }
        
        [Required(ErrorMessage = "Podaj pensję")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Podaj poprawną kwotę")]
        public double Pensja { get; set; }

        //Navigation Properties
        public List<Rezerwacja> Rezerwacje { get; set; } = new List<Rezerwacja>();
    }
}
