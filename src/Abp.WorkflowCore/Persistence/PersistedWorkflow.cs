using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkflowCore.Models;

namespace Abp.Workflow.Persistence
{
    [Table("Workflows")]
    public class PersistedWorkflow : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        [MaxLength(100)]
        public string WorkflowDefinitionId { get; set; }

        
        public PersistedWorkflowDefinition WorkflowDefinition { get; set; }

        public int Version { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [MaxLength(200)]
        public string Reference { get; set; }

        public virtual PersistedExecutionPointerCollection ExecutionPointers { get; set; } = new PersistedExecutionPointerCollection();

        public long? NextExecution { get; set; }

        public string Data { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? CompleteTime { get; set; }

        public WorkflowStatus Status { get; set; }
        public int? TenantId { get; set; }

        public string CreateUserIdentityName { get; set; }
    }
}
