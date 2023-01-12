using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace TorKartingowyCoreMVC.Models
{
    public class Gokart
    {
        [Key] 
        public int Numer { get; set; }

        [Required(ErrorMessage = "Wybierz typ gokarta")]
        public string Typ { get; set; }

        [Required(ErrorMessage = "Podaj model gokarta")]
        public string Model { get; set; }
    }
}
