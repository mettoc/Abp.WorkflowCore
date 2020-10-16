using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using WorkflowDemo.Configuration;
using WorkflowDemo.Web;

namespace WorkflowDemo.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class WorkflowDemoDbContextFactory : IDesignTimeDbContextFactory<WorkflowDemoDbContext>
    {
        public WorkflowDemoDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<WorkflowDemoDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            WorkflowDemoDbContextConfigurer.Configure(builder, configuration.GetConnectionString(WorkflowDemoConsts.ConnectionStringName));

            return new WorkflowDemoDbContext(builder.Options);
        }
    }
}
