
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogoFilmes;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> option) : base(option) { }

    public DbSet<Genero>? Generos { get; set; }
    public DbSet<Filme>? Filmes { get; set; }

    protected override void OnModelCreating(ModelBuilder db)
    {
        // Mapeando as props como devem ir para o db
        // Gênero
        db.Entity<Genero>().HasKey(g => g.GeneroId);
        db.Entity<Genero>().Property(g => g.Nome).HasMaxLength(100).IsRequired();

        // Filmes
        db.Entity<Filme>().HasKey(f => f.FilmeId);
        db.Entity<Filme>().Property(f => f.Nome).HasMaxLength(150).IsRequired();
        db.Entity<Filme>().Property(f => f.Sinopse).HasMaxLength(300);

        // Relacionamento entre eles

        db.Entity<Filme>().HasOne<Genero>(g => g.Genero).WithMany(f => f.Filmes).HasForeignKey(g => g.GeneroId);


    }
}
