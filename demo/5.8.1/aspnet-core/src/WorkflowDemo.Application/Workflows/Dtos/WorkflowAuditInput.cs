using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WorkflowDemo.Application.Workflows.Dtos
{
    public class WorkflowAuditInput
    {
        public string ExecutionPointerId { get; set; }

        public bool Pass { get; set; }
        [MaxLength(500)]
        public string Remark { get; set; }

    }
}
