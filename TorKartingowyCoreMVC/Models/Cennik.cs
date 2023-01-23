using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TorKartingowyCoreMVC.Models
{
    public class Cennik
    {
        [Key]
        public int Id { get; set; }
        [Required (ErrorMessage = "Podaj cenę za gokart")]
        public double Spalinowy { get; set; }
        [Required(ErrorMessage = "Podaj cenę za gokart")]
        public double Elektryczny { get; set; }

        [Required (ErrorMessage = "Podaj cenę za gokart")]
        public double DlaDzieci { get; set; }

        [Required (ErrorMessage = "Podaj cenę za szkolenie")]
        public double DodatkoweSzkolenie { get; set; }
    }
}
