using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace clothing_store.infrastructure.context
{
    public class DbContextFactory : IDesignTimeDbContextFactory<dbContext>
    {
        public dbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<dbContext>();
            var connectionString = "Server=LAPTOP-RAKVSLVC;Database=sushmitha;Integrated Security=True;TrustServerCertificate=True";
            optionsBuilder.UseSqlServer(connectionString);

            return new dbContext(optionsBuilder.Options);
        }
    }
}
