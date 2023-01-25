using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace TorKartingowyCoreMVC.Models
{
    public class DostepneGodziny
    {
        [Key]
        public string TorData { get; set; }
        public int G09 { get; set; } = 0;
        public int G10 { get; set; } = 0;
        public int G11 { get; set; } = 0;
        public int G12 { get; set; } = 0;
        public int G13 { get; set; } = 0;
        public int G14 { get; set; } = 0;
        public int G15 { get; set; } = 0;
        public int G16 { get; set; } = 0;
        public int G17 { get; set; } = 0;
        public int G18 { get; set; } = 0;
        public int G19 { get; set; } = 0;
        public int G20 { get; set; } = 0;
    }
}
