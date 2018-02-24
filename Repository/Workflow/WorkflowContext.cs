using Microsoft.EntityFrameworkCore;

namespace dotnet_cpnucleo_pages.Repository.Workflow
{
    public class WorkflowContext : DbContext
    {
        public WorkflowContext(DbContextOptions<WorkflowContext> options)
            : base(options)
        { }

        public DbSet<WorkflowItem> Workflows { get; set; }   
    } 
}