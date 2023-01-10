using AHM.Domain.Workflow.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.Innovation
{
    public interface IInnovationWorkflow
    {
        Task<string> RegisterWorkflow(WfRequest request, WfExecutor executor);
        Task StartWorkflow(string wfguid, WfExecutor executor);
        Task<string> StartTask(string wfguid, string taskId, WfExecutor executor);
    }
}
