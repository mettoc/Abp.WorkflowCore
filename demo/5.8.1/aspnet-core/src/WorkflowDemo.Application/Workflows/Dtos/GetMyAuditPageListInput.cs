using Abp.Application.Services.Dto;
using Abp.BaseDto;
using System;
using WorkflowDemo.Core.Workflows;

namespace WorkflowDemo.Application.Workflows.Dtos
{
    public class GetMyAuditPageListInput : PagedInputDto
    {
        /// <summary>
        /// 
        /// </summary>
        public bool? AuditedMark { get; set; }
    }
    public class GetMyAuditPageListOutput : EntityDto<Guid>
    {
        public string WorkflowDefinitionId { get; set; }
        public Guid WorkflowId { get; set; }
        public int Version { get; set; }

        public int StepId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ExecutionPointerId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 流程名
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        public EnumAuditStatus Status { get; set; }

        public DateTime? AuditTime { get; set; }

    }
}
