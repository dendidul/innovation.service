using AHM.Domain.Ahmhrirs.Entities;
using AHM.Domain.Ahmhrirs.Entities.Innovation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.Innovation
{
    public interface IHRTxnInnovationContext
    {
      //  Task<List<AhmhrirsTxnip>> GetStatusIp(string v_nid, string v_perid, string v_vnrp, string v_vstatus, CancellationToken cancellationToken);
        Task<string> GetForbiddenCharacter(CancellationToken cancellationToken);
        Task<string> getGolonganKaryawan(string NRP, CancellationToken cancellationToken);
        Task<int> GetRunNo(Int64 _inrp, CancellationToken cancellationToken);
        Task<string> GetRegistrationNum(Int64 _inrp, string themeType, string AreaId, CancellationToken cancellationToken);


        Task<string> GetPathAttachment(string param4, CancellationToken cancellationToken);

        Task<string> GetDownloadLinkIp(string fileName, CancellationToken cancellationToken);
        Task<string> GetNrpAtasan(string pnrp, Int64 _inrp, CancellationToken cancellationToken);

        Task<AHMHRIRS021_InfoSect> GetInfoSectionByNrp(string pnrp, int usr, Int64 _inrp, CancellationToken cancellationToken);

        Task<string> GetStatusByNrp(string _inrp, CancellationToken cancellationToken);

        Task<string> GetAreaByNrp(Int64 _inrp, CancellationToken cancellationToken);
    }

}
