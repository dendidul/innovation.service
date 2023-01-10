using Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.Innovation
{
    public interface IItwfsInnovationContext
    {
        Task<string> CekDelegasiApproval(string pnrp, CancellationToken cancellationToken);
        Task<string> GetLastTaskid(string WFGuid, CancellationToken cancellationToken);

        
       
        Task<List<HistoryTable>> GetHistoryWorkflow(string vwfguid,string noreg, CancellationToken cancellationToken);
        string GetTaskIde(string wfId, string eventType, string taskId);
    }
}
