
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TorKartingowyCoreMVC.Models;

namespace TorKartingowyCoreMVC.Data;
public class ApplicationDbContext :IdentityDbContext
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
	{
	}

	public DbSet<Klient> Klienci { get; set; }
	public DbSet<Pracownik> Pracownicy { get; set;}
	public DbSet<Rezerwacja> Rezerwacje { get; set; }
	public DbSet<Tor> Tory { get; set; }
	public DbSet<Gokart> Gokarty { get; set; }
	public DbSet<Serwis> Serwisy { get; set; }
	public DbSet<RejestrPrac> RejestrPrac { get; set; }
	public DbSet<DostepneGodziny> DostepneGodziny { get; set; }
	public DbSet<IloscGokartow> IloscGokartow { get; set; }
	public DbSet<Harmonogram> Harmonogram { get; set; }
	public DbSet<Platnosc> Platnosci { get; set; }
	public DbSet<Cennik> Cennik { get; set;}
}
