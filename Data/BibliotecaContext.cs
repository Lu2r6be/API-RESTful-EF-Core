using BibliotecaApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaApi.Data;

public class BibliotecaContext(DbContextOptions<BibliotecaContext> options) : DbContext(options)
{
    public DbSet<Livro> Livros => Set<Livro>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Livro>(entity =>
        {
            entity.HasKey(livro => livro.Id);
            entity.Property(livro => livro.Titulo).HasMaxLength(200).IsRequired();
            entity.Property(livro => livro.Autor).HasMaxLength(150).IsRequired();
        });
    }
}
