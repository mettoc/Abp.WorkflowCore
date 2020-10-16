using Abp.Application.Services.Dto;
using Abp.Workflow;
using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Models;

namespace WorkflowDemo.Application.Workflows.Dtos
{
    public class WorkflowDto:EntityDto<Guid>
    {
        public string WorkflowDefinitionId { get; set; }
        public int Version { get; set; }
        /// <summary>
        /// 流程定义输入的数据
        /// </summary>
        public IEnumerable<IEnumerable<IEnumerable<WorkflowFormData>>> Inputs { get; set; }

        /// <summary>
        /// 流程输入数据
        /// </summary>
        public Dictionary<string, object> Data { get; set; }
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
        public DateTime? CompleteTime { get; set; }
        public WorkflowStatus Status { get; set; }

        public IEnumerable<WorkflowExecutionRecord> ExecutionRecords { get; set; }
    }
    public class WorkflowExecutionRecord
    {
        public string ExecutionPointerId { get; set; }
        public string StepName { get; set; }
        public int  StepId { get; set; }
        public string  StepTitle { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
