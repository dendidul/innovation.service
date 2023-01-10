using AHM.Domain.Workflow.Entities;
using AHM.ExternalService.Workflow;
using Core.Application.Interfaces.Innovation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ExternalService.Workflow
{
    public class InnovationWorkflow : IInnovationWorkflow
    {
        private readonly RegisterWorkflowHelper _registerWorkflowHelper;
        private readonly StartWorkflowHelper _startWorkflowHelper;
        private readonly StartTaskHelper _startTaskHelper;

        public InnovationWorkflow(RegisterWorkflowHelper registerWorkflowHelper, StartWorkflowHelper startWorkflowHelper, StartTaskHelper startTaskHelper)
        {
            _registerWorkflowHelper = registerWorkflowHelper;
            _startWorkflowHelper = startWorkflowHelper;
            _startTaskHelper = startTaskHelper;
        }


        public async Task<string> RegisterWorkflow(WfRequest request, WfExecutor executor)
        {
            var response = await _registerWorkflowHelper.RegisterWorkflow(request, executor);
            return response.Data.WorkflowGuid;
        }

        public async Task<string> StartTask(string wfguid, string taskId, WfExecutor executor)
        {
            var response = await _startTaskHelper.StartTask(wfguid, taskId, executor);
            return response.Data.TaskGuid;
        }

        public async Task StartWorkflow(string wfguid, WfExecutor executor)
        {
            await _startWorkflowHelper.StartWorkflow(wfguid, executor);
        }
    }
}
