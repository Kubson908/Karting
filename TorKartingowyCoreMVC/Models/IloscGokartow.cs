using System.ComponentModel.DataAnnotations;


namespace TorKartingowyCoreMVC.Models
{
    public class IloscGokartow
    {
        [Key]
        public string DataGodzina { get; set; }
        
        public int Spalinowe { get; set; } = 0;
        public int Elektryczne { get; set; } = 0;
        public int DlaDzieci { get; set; } = 0;
    }
}
