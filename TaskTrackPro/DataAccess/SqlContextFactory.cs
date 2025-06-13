using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DataAccess
{
    public class SqlContextFactory : IDesignTimeDbContextFactory<SqlContext>
    {
        public SqlContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SqlContext>();

            optionsBuilder.UseSqlServer(
                "Server=localhost,1433;Database=TaskTrackProDB;User Id=sa;Password=Contraseña123;TrustServerCertificate=true");

            return new SqlContext(optionsBuilder.Options);
        }
    }
}