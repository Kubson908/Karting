using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TorKartingowyCoreMVC.Models
{
    public class Platnosc
    {
        [Key]
        public int Numer { get; set; }

        public string? Sposob { get; set; } = null;

        public double Kwota { get; set; }

        public bool Status { get; set; }
        public string? RodzajDokumentu { get; set; } = null;
    }
}