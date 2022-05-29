using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace BTIT.EPM.EntityFrameworkCore
{
    public static class EPMDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<EPMDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<EPMDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}