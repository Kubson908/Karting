using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace TorKartingowyCoreMVC.Models
{
    public class Klient
    {
        [Key]
        public int Numer { get; set; }

        [Required(ErrorMessage = "Podaj adres e-mail"), DisplayName("Adres e-mail")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Błędny adres e-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Ustaw odpowiednie hasło"), DisplayName("Hasło")] 
        public string Haslo { get; set; }
        [Required(ErrorMessage = "Podaj imię"), DisplayName("Imię")]
        public string Imie { get; set; }
        [Required(ErrorMessage = "Podaj nazwisko")]
        public string Nazwisko { get; set; }
        [Required(ErrorMessage = "Podaj numer telefonu")]
        [StringLength(9)]
        [MinLength(9, ErrorMessage = "Błędny numer telefonu")]
        public string Telefon { get; set; }
        
        public bool? Szkolenie { get; set; } = false;
        public float? Rekord { get; set; }

        //Navigation Properties
        public List<Rezerwacja> Rezerwacje { get; set; } = new List<Rezerwacja>();
    }
}
