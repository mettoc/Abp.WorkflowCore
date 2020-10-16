using Abp.Workflow;
using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Abp.WorkflowCore.Test
{
    public class HelloAbpWorkflow : AbpWorkflow
    {
        public HelloAbpWorkflow()
        {
            this.Id = "HelloAbpWorkflow";
            this.Version = 1;
        }

        public override void Build(IWorkflowBuilder<WorkflowParamDictionary> builder)
        {
            builder.StartWith<SayHelloStepBody>().Then<SayGoodByStepBody>();
        }
    }
    public class SayHelloStepBody : StepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine("Hello!!");
            return ExecutionResult.Next();
        }
    }
    public class SayGoodByStepBody : StepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine("goodby!!");
            return ExecutionResult.Next();
        }
    }
}
