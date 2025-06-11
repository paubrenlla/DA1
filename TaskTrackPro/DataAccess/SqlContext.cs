using Domain;
using Microsoft.EntityFrameworkCore;


namespace DataAccess;

public class SqlContext : DbContext
{
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Recurso> Recursos { get; set; }

    public SqlContext(DbContextOptions<SqlContext> options) : base(options)
    {
        if(!Database.IsInMemory()) this.Database.Migrate();
    }
}