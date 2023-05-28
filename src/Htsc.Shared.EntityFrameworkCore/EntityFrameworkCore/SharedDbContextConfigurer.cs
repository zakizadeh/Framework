using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Htsc.Shared.EntityFrameworkCore
{
    public static class SharedDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<SharedDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<SharedDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}