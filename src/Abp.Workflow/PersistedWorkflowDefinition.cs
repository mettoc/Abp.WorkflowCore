using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abp.Workflow
{
    [Table("WorkflowDefinitions")]
    public class PersistedWorkflowDefinition : FullAuditedEntity<string>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public string Title { get; set; }
        public int Version { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }

        public string Group { get; set; }

        /// <summary>
        /// 输入
        /// </summary>
        public IEnumerable<IEnumerable<IEnumerable<WorkflowFormData>>> Inputs { get; set; }
        /// <summary>
        /// 流程节点
        /// </summary>
        public IEnumerable<WorkflowNode> Nodes { get; set; }
        public PersistedWorkflowDefinition(string id, string title, int version, IEnumerable<IEnumerable<IEnumerable<WorkflowFormData>>> inputs, IEnumerable<WorkflowNode> nodes, int? tenantId = null)
        {
            this.Id = id;
            this.Title = title;
            this.Version = version;
            this.Inputs = inputs;
            this.Nodes = nodes;
            this.TenantId = tenantId;
        }
    }
   
}
