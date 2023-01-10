using AHM.Common.Helper.Encryption;
using AHM.Domain.Ahmhrirs.Entities.Innovation;
using Core.Application.Interfaces.Innovation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Features.Innovation.Queries
{
    public class GetDetIPDataByQuery : IRequest<GetDetIPResponseViewModel>
    {
        public string vnrp { get; set; }
        public string vnoreg { get; set; }
      

        public string domain { get; set; }
        public string username { get; set; }
        public string zone { get; set; }

        public class GetDetIPDataByQueryHandler : IRequestHandler<GetDetIPDataByQuery, GetDetIPResponseViewModel>
        {
            private readonly IHRTxnInnovationContext _context;

            private readonly IHrirsInnovationContext _hrirscontext;
            private readonly IEncryptionHelper _encryptionHelper;

            public GetDetIPDataByQueryHandler(IEncryptionHelper encryptionHelper, IHRTxnInnovationContext context, IHrirsInnovationContext hrirscontext)
            {
                _context = context;
                _hrirscontext = hrirscontext; ;
                _encryptionHelper = encryptionHelper;
            }

            public async Task<GetDetIPResponseViewModel> Handle(GetDetIPDataByQuery request, CancellationToken cancellationToken)
            {
                GetDetIPResponseViewModel obj = new GetDetIPResponseViewModel();

                List<AHMHRIRS021_LISTIP> listdata = await _hrirscontext.GetBeforeAfter(request.vnoreg, request.vnrp, cancellationToken);
                List<AHMHRIRS021_ATTACHMENTDETIL> dataAndAtt = new List<AHMHRIRS021_ATTACHMENTDETIL>();

                string lengthTime = DateTime.Now.ToFileTime().ToString() + "_";
                string tempFileSubstr = string.Empty;
                string pathFolder = await _context.GetPathAttachment("1", cancellationToken);

                var vphone = await _hrirscontext.getPhone(request.vnrp, request.vnoreg, cancellationToken);
                var resultIP = await _hrirscontext.getDataIP(request.vnoreg, cancellationToken);


                foreach (AHMHRIRS021_LISTIP ip in listdata)
                {
                    string vfilenameid = "";
                    DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(await _context.GetPathAttachment("1", cancellationToken));
                    string VNoDocSplit = ip.XIP_VNODOC.Substring(0, 5);
                    if (ip.NBEFAFT != 3)
                    {
                        string vfileName = string.Empty;

                        if (!string.IsNullOrEmpty(ip.VFILE))
                        {
                            FileInfo[] filesInDir = hdDirectoryInWhichToSearch.GetFiles("*" + VNoDocSplit + "-" + ip.VNRP + "-IPS-" + VNoDocSplit + "-" + (ip.NBEFAFT == 1 ? "BEFORE" : "AFTER") + "-1" + "*.*");

                            foreach (FileInfo foundFile in filesInDir)
                            {
                                vfilenameid = foundFile.FullName;
                                vfileName = Path.GetFileName(vfilenameid);
                                string nonName = vfilenameid.Split('-')[0].ToString() + '-' + vfilenameid.Split('-')[1].ToString() +
                                    '-' + vfilenameid.Split('-')[2].ToString() + '-' + vfilenameid.Split('-')[3].ToString() +
                                    '-' + vfilenameid.Split('-')[4].ToString() + '-';
                                tempFileSubstr = vfilenameid.Replace(nonName, "");

                                if (vfilenameid.Contains(VNoDocSplit) && tempFileSubstr.Contains(ip.VFILE))
                                {
                                    if (tempFileSubstr.ToLower().Trim() == ip.VFILE.ToLower().Trim())
                                    {

                                        string pathdata = await _context.GetPathAttachment("1", cancellationToken);

                                        string userFolder = request.zone + "_" + _encryptionHelper.MD5Hash(request.domain + "\\" + request.username);
                                        dataAndAtt.Add(new AHMHRIRS021_ATTACHMENTDETIL
                                        {
                                            XIP_VNODOC = ip.XIP_VNODOC,
                                            NBEFAFT = ip.NBEFAFT,
                                            NSEQ = ip.NSEQ,
                                            VCOND = ip.VCOND,
                                            VATTCHNAMEID = vfileName == "" ? "" : vfileName,
                                            VPATH = await _hrirscontext.GetAttachmentId(await trimForbidden(ip.VFILE, cancellationToken), ip.XIP_VNODOC, cancellationToken),
                                            VFILE = _hrirscontext.GetDownloadLinkIp(userFolder, pathdata, request.domain, request.username, request.zone, vfileName, cancellationToken)
                                        });
                                        //logket += "nonstd/file " + ip.VCOND + ip.NBEFAFT + ip.NSEQ + ip.VFILE + vfileName;
                                        break;
                                    }

                                    //else
                                    //{


                                    //}
                                }
                            }
                        }
                        else
                        {
                            string pathdata = await _context.GetPathAttachment("1", cancellationToken);
                            string userFolder = request.zone + "_" + _encryptionHelper.MD5Hash(request.domain + "\\" + request.username);
                            dataAndAtt.Add(new AHMHRIRS021_ATTACHMENTDETIL
                            {
                                XIP_VNODOC = ip.XIP_VNODOC,
                                NBEFAFT = ip.NBEFAFT,
                                NSEQ = ip.NSEQ,
                                VCOND = ip.VCOND,
                                VATTCHNAMEID = vfileName == "" ? "" : vfileName,
                                VPATH = await _hrirscontext.GetAttachmentId(await trimForbidden(ip.VFILE, cancellationToken), ip.XIP_VNODOC, cancellationToken),
                                VFILE = _hrirscontext.GetDownloadLinkIp(userFolder, pathdata, request.domain, request.username, request.zone, vfileName, cancellationToken)
                            });
                        }
                    }
                    else
                    {
                        string vfileName = string.Empty;

                        if (!string.IsNullOrEmpty(ip.VFILE))
                        {
                            FileInfo[] filesInDir = hdDirectoryInWhichToSearch.GetFiles("*" + VNoDocSplit + "-" + ip.VNRP + "-IPS-" + VNoDocSplit + "-" + "STANDARD" + "-1" + "*.*");
                            //FileInfo foundFile = filesInDir.FirstOrDefault();

                            foreach (FileInfo foundFile in filesInDir)
                            {
                                vfilenameid = foundFile.FullName;
                                vfileName = Path.GetFileName(vfilenameid);
                                string nonName = vfilenameid.Split('-')[0].ToString() + '-' + vfilenameid.Split('-')[1].ToString() +
                                    '-' + vfilenameid.Split('-')[2].ToString() + '-' + vfilenameid.Split('-')[3].ToString() +
                                    '-' + vfilenameid.Split('-')[4].ToString() + '-';
                                tempFileSubstr = vfilenameid.Replace(nonName, "");

                                if (vfilenameid.Contains(VNoDocSplit) && tempFileSubstr.Contains(ip.VFILE))
                                {
                                    vfileName = Path.GetFileName(vfilenameid);
                                    if (tempFileSubstr.ToLower().Trim() == ip.VFILE.ToLower().Trim())
                                    {
                                        string pathdata = await _context.GetPathAttachment("1", cancellationToken);
                                        string userFolder = request.zone + "_" + _encryptionHelper.MD5Hash(request.domain + "\\" + request.username);

                                        dataAndAtt.Add(new AHMHRIRS021_ATTACHMENTDETIL
                                        {
                                            XIP_VNODOC = ip.XIP_VNODOC,
                                            NBEFAFT = ip.NBEFAFT,
                                            NSEQ = ip.NSEQ,
                                            VCOND = ip.VCOND,
                                            VATTCHNAMEID = vfileName == "" ? "" : vfileName,
                                            VPATH = await _hrirscontext.GetAttachmentId(await trimForbidden(ip.VFILE, cancellationToken), ip.XIP_VNODOC, cancellationToken),
                                            VFILE = _hrirscontext.GetDownloadLinkIp(userFolder, pathdata, request.domain, request.username, request.zone, vfileName, cancellationToken)

                                        });
                                        //logket += "std/file " + ip.VCOND + ip.NBEFAFT + ip.NSEQ + ip.VFILE + vfileName;
                                        break;
                                    }
                                }
                            }
                        }

                        else
                        {
                            string pathdata = await _context.GetPathAttachment("1", cancellationToken);
                            string userFolder = request.zone + "_" + _encryptionHelper.MD5Hash(request.domain + "\\" + request.username);

                            dataAndAtt.Add(new AHMHRIRS021_ATTACHMENTDETIL
                            {
                                XIP_VNODOC = ip.XIP_VNODOC,
                                NBEFAFT = ip.NBEFAFT,
                                NSEQ = ip.NSEQ,
                                VCOND = ip.VCOND,
                                VATTCHNAMEID = vfileName == "" ? "" : vfileName,
                                VPATH = await _hrirscontext.GetAttachmentId(await trimForbidden(ip.VFILE, cancellationToken), ip.XIP_VNODOC, cancellationToken),
                                VFILE = _hrirscontext.GetDownloadLinkIp(userFolder, pathdata, request.domain, request.username, request.zone, vfileName, cancellationToken)

                            });
                        }
                    


                    }




                }

                obj.ATTACTHMENTS = dataAndAtt.Distinct().ToList();

                return obj;
            }

            private async Task<string> trimForbidden(string name, CancellationToken cancellationToken)
            {
                var getData = await _context.GetForbiddenCharacter(cancellationToken);
                string[] chars = getData.Split(',');
                foreach (string c in chars)
                {
                    name = name.Replace(c, "");
                }
                return name;

            }

            public async Task<string> GetDownloadLinkIp(string domain, string username, string zone, string fileName, CancellationToken cancellationToken)
            { //nitip update

                string userFolder = zone + "_" + _encryptionHelper.MD5Hash(domain + "\\" + username);

                string path = await _context.GetPathAttachment("1", cancellationToken);
                // path = Path.Combine(path, userFolder);

                string fullPath = Path.Combine(path, fileName);

                return "handlers/file.ashx?FilePathIp=" + fullPath + "&urlparam=FilePathIp";
            }



        }
    }
}
