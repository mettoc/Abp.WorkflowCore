using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace WorkflowDemo.EntityFrameworkCore
{
    public static class WorkflowDemoDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<WorkflowDemoDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<WorkflowDemoDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
