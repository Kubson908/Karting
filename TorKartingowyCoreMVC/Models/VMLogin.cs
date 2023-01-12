namespace TorKartingowyCoreMVC.Models
{
    public class VMLogin
    {
        public int Numer { get; set; }
        public string Email { get; set; }
        public string Haslo { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string Telefon { get; set; }
        public bool? Szkolenie { get; set; }
        public float? Rekord { get; set; }
        public bool KeepLoggedIn { get; set; }
    }
}
