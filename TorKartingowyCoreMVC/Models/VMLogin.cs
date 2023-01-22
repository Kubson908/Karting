using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TorKartingowyCoreMVC.Models
{
    public class VMLogin
    {
        public int Numer { get; set; }

        [Required(ErrorMessage = "Podaj adres e-mail"), DisplayName("Adres e-mail")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Błędny adres e-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Wprowadź hasło"), DisplayName("Hasło")]
        public string Haslo { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string Telefon { get; set; }
        public bool? Szkolenie { get; set; }
        public float? Rekord { get; set; }
        public bool KeepLoggedIn { get; set; }
    }
}
