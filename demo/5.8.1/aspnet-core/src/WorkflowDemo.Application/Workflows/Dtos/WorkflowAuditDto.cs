using System;
using System.Collections.Generic;
using WorkflowDemo.Core.Workflows;

namespace WorkflowDemo.Application.Workflows.Dtos
{
    public class WorkflowAuditDto 
    {
        /// <summary>
        /// 是否需要审核
        /// </summary>
        public bool NeedAudit { get; set; }

        /// <summary>
        /// 审核记录
        /// </summary>
        public Dictionary<string, IEnumerable<WorkflowAuditRecord>> AuditRecords { get; set; }
    }

    public class WorkflowAuditRecord
    {
        public long UserId { get; set; }
        public string ExecutionPointerId { get; set; }
        public string UserIdentityName { get; set; }
        public string UserHeadPhoto { get; set; }
        public EnumAuditStatus Status { get; set; }
        public DateTime? AuditTime { get; set; }
        public string Remark { get; set; }
    }
}
