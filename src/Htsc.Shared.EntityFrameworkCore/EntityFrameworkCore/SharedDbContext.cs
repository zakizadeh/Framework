using Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Htsc.Shared.EntityFrameworkCore
{
    public class SharedDbContext : AbpDbContext
    {
        /* Define an IDbSet for each entity of the application */


        public SharedDbContext(DbContextOptions<SharedDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SharedDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
