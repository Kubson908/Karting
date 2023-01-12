namespace TorKartingowyCoreMVC.Models
{
    public class VMPracownik
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Haslo { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string Stanowisko { get; set; }
        public string Telefon { get; set; }
        public double Pensja { get; set; }
        public bool KeepLoggedIn { get; set; }
    }
}
