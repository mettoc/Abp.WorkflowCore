using Abp.Json;
using Abp.Workflow;
using Abp.Workflow.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.WorkflowCore.Persistence.EntityFrameworkCore
{
    public static class ModelBuilderExtension
    {
        public static ModelBuilder ConfigWorkflowCore(this ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<PersistedWorkflowDefinition>();
            builder.Property(u => u.Version).HasDefaultValue(1);
            builder.HasKey(u => new { u.Id, u.Version });
            builder.Property(u => u.Title).HasMaxLength(256);
            builder.Property(u => u.Group).HasMaxLength(100);
            builder.Property(u => u.Icon).HasMaxLength(50);
            builder.Property(u => u.Color).HasMaxLength(50);
            builder.Property(u => u.Id).HasMaxLength(100);
            builder.Property(u => u.Inputs).HasConversion(u => u.ToJsonString(false, false), u => u.FromJsonString<IEnumerable<IEnumerable<IEnumerable<WorkflowFormData>>>>());
            builder.Property(u => u.Nodes).HasConversion(u => u.ToJsonString(false, false), u => u.FromJsonString<IEnumerable<WorkflowNode>>());

            modelBuilder.Entity<PersistedWorkflow>().HasOne(u => u.WorkflowDefinition).WithMany().HasForeignKey(u => new { u.WorkflowDefinitionId, u.Version });
            return modelBuilder;
        }
    }
}
