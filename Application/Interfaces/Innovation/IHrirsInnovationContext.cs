using AHM.Domain.Ahmhrirs.Entities;
using AHM.Domain.Ahmhrirs.Entities.Innovation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.Innovation
{
    public interface IHrirsInnovationContext
    {
        Task<List<AhmhrirsTxnip>> GetStatusIp(string v_vnrp, string v_vstatus, CancellationToken cancellationToken);
        Task<string> getPhone(string vnrp, string vnoreg, CancellationToken cancellationToken);
        Task<AhmhrirsTxnip> getDataIP(string vnoreg, CancellationToken cancellationToken);
        Task<string> GetUploadedFilePath(string encrypted, string username, string domain, string zone, string fileId, string path, CancellationToken cancellationToken);

        Task<string> GetWFIDByNodoc(string nodoc, CancellationToken cancellationToken);
        Task<List<string>> GetArea(string varea, CancellationToken cancellationToken);

        Task<List<AHMHRIRS021_LISTIP>> GetBeforeAfter(string pnoreg, string pnrp, CancellationToken cancellationToken);

        Task<string> GetAttachmentId(string vname, string vnodoc, CancellationToken cancellationToken);

        Task<string> GetThemeType(CancellationToken cancellationToken);

        Task<AHMHRIRS021_AREA> GetAreaIDByNrp(Int64 _inrp, string VVAL1, CancellationToken cancellationToken);

        Task<string> GetPeriodID(CancellationToken cancellationToken);
        Task<DateTime> GetDateSubmitByVwfguid(string VWFGUID, CancellationToken cancellationToken);

        string GetDownloadLinkIp(string userFolder, string path, string domain, string username, string zone, string fileName, CancellationToken cancellationToken);
        Task<List<string>> getSettingValue(string key, CancellationToken cancellationToken);

        string GetPeriodAktif(CancellationToken cancellationToken);
        Task<AhmhrirsDtlformset> GetGolonganSets(CancellationToken cancellationToken);
        void InserDataHeaderDetail(string employeeStatus, AHMHRIRS021_IPDATA dataip, List<AHMHRIRS021_Attachment> detail, bool isSend, string wfguid, Int64 _inrp, CancellationToken cancellationToken);
        void UpdateDataHeaderDetail(string lastTaskId, AHMHRIRS021_IPDATA dataip, List<AHMHRIRS021_Attachment> detail, bool isSend, string wfguid, Int64 _inrp, CancellationToken cancellationToken);

        void UpdateAttachmentId(AHMHRIRS021_IPDATA dataip, List<AHMHRIRS021_Attachment> detail, bool isSend, CancellationToken cancellationToken);
        Task<List<AhmhrirsTxnip>> GetTotalGrade(string vnrp, string periodId, CancellationToken cancellationToken);

        }
}
