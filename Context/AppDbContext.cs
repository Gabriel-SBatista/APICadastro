using APICadastro.Models;
using Microsoft.EntityFrameworkCore;

namespace APICadastro.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {}

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Inativacao> Inativacoes { get; set;}
    public DbSet<TipoAcesso> TiposAcesso { get; set; }
}
