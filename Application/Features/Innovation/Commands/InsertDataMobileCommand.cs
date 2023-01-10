using AHM.Common.Helper.Config;
using AHM.Common.Helper.Encryption;
using AHM.Domain.Ahmhrirs.Entities.Innovation;
using AHM.Domain.Workflow.Entities;
using Core.Application.Interfaces.Innovation;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Features.Innovation.Commands
{
    public class InsertDataMobileCommand : IRequest<string>
    {

        public string vkondisiaf { get; set; }
        public string vkondisibf { get; set; }
        public string vmoral { get; set; }

        public string vnama { get; set; }
        public string vnoekst { get; set; }
        public string vnid { get; set; }
        public string vdivid { get; set; }
        public string vnoreg { get; set; }
        public string vnrp { get; set; }
        public string vdiv { get; set; }
        public string vdirid { get; set; }
        public string vdir { get; set; }
        public string vdeptid { get; set; }
        public string vdept { get; set; }
        public string vdeliv { get; set; }
        public string vcreatedate { get; set; }
        public string vcost { get; set; }
        public string varea { get; set; }
        public string vakibat { get; set; }

        public string myguid { get; set; }


        public string vproses { get; set; }
        public string vquality { get; set; }
        public string vsafety { get; set; }
        public string vseksi { get; set; }
        public string vseksiid { get; set; }
        public string vtema { get; set; }
        public string vstandar { get; set; }
        public string vsubdept { get; set; }
        public string vsubdeptid { get; set; }
        public string v_action { get; set; }


        public string[] VNOTE { get; set; }
        public string[] VATTCHNAME { get; set; }
        public string[] VATTCHNAMEONLY { get; set; }
        public string[] VATTCHNAMEID { get; set; }
        public string[] VSEQ { get; set; }
        public string[] VFLAG { get; set; }
        public string[] VTYPE { get; set; }

        public string[] VPATH { get; set; }
        public string nrp { get; set; }
        public string domain { get; set; }
        public string userName { get; set; }
        public string displayName { get; set; }
        public string Email { get; set; }
        public string Zone { get; set; }

        public string IpAddress { get; set; }
        public class InsertDataMobileCommandHandler : IRequestHandler<InsertDataMobileCommand, string>
        {


            private readonly IHRTxnInnovationContext _context;

            private readonly IHrirsInnovationContext _hrirscontext;
            private readonly IEncryptionHelper _encryptionHelper;
            private readonly IItwfsInnovationContext _IItwfscontext;
            private readonly IConfiguration _configuration;
            private readonly IInnovationWorkflow _workflow;

            private readonly Iitb2eInnovationContext _iitb2EInnovationContext;
            private readonly IConfigCreator _configCreator;

            private string _vappid;
            private string _vformid;
            private string _workflowId;

            public InsertDataMobileCommandHandler(IConfigCreator configCreator, IInnovationWorkflow workflow, IConfiguration configuration, Iitb2eInnovationContext iitb2EInnovationContext, IItwfsInnovationContext IItwfscontext, IEncryptionHelper encryptionHelper, IHRTxnInnovationContext context, IHrirsInnovationContext hrirscontext)
            {
                _context = context;
                _hrirscontext = hrirscontext; ;
                _encryptionHelper = encryptionHelper;
                _IItwfscontext = IItwfscontext;
                _iitb2EInnovationContext = iitb2EInnovationContext;
                _configuration = configuration;
                _workflow = workflow;
                _vappid = _configCreator.Get("Workflow:Innovation:AppId");
                _vformid = _configCreator.Get("Workflow:Innovation:FormId");
                _workflowId = _configCreator.Get("Workflow:Innovation:WorkflowId");

            }

            public async Task<string> Handle(InsertDataMobileCommand request, CancellationToken cancellationToken)
            {
                string result = "";


                string vakibat = request.vakibat ?? "";
                string varea = request.varea ?? "";
                string vcost = request.vcost ?? "";
                string vcreatedate = request.vcreatedate ?? "";
                string vdeliv = request.vdeliv ?? "";
                string vdept = request.vdept ?? "";
                string vdeptid = request.vdeptid ?? "";
                string vdir = request.vdir ?? "";
                string vdirid = request.vdirid ?? "";
                string vdiv = request.vdiv ?? "";
                string vdivid = request.vdivid ?? "";
                string vkondisiaf = request.vkondisiaf ?? "";
                string vkondisibf = request.vkondisibf ?? "";
                string vmoral = request.vmoral ?? "";
                string vnama = request.vnama ?? "";
                string vnoekst = request.vnoekst ?? "";
                string vnid = request.vnid ?? "";
                string vnoreg = request.vnoreg ?? "";
                string vnrp = request.vnrp ?? "";
                string vproses = request.vproses ?? "";
                string vquality = request.vquality ?? "";
                string vsafety = request.vsafety ?? "";
                string vseksi = request.vseksi ?? "";
                string vseksiid = request.vseksiid ?? "";
                string vtema = request.vtema ?? "";
                string vstandar = request.vstandar ?? "";
                string vsubdept = request.vsubdept ?? "";
                string vsubdeptid = request.vsubdeptid ?? "";
                string v_action = request.v_action ?? "";
                vnoreg = vnoreg.Trim();

                string[] VNOTE = request.VNOTE ?? null;
                string[] VATTCHNAME = request.VATTCHNAME ?? null;
                string[] VATTCHNAMEONLY = request.VATTCHNAMEONLY ?? null;
                string[] VATTCHNAMEID = request.VATTCHNAMEID ?? null;
                string[] VSEQ = request.VSEQ ?? null;
                string[] VFLAG = request.VFLAG ?? null;
                string[] VTYPE = request.VTYPE ?? null;

                string[] VPATH = request.VPATH ?? null;

                string myguid = request.myguid ?? "";
                var vGolongan = _context.getGolonganKaryawan(vnrp, cancellationToken);

                List<AHMHRIRS021_Attachment> listAtt = new List<AHMHRIRS021_Attachment>();

                if (VNOTE != null)
                {
                    for (int j = 0; j < VNOTE.Length; j++)
                    {
                        listAtt.Add(new AHMHRIRS021_Attachment()
                        {
                            VNOTE = VNOTE != null ? VNOTE[j] : "",
                            VATTCHNAME = VATTCHNAME != null ? await trimForbidden(VATTCHNAME[j], cancellationToken) : "",
                            VATTCHNAMEONLY = VATTCHNAMEONLY != null ? await trimForbidden(VATTCHNAMEONLY[j], cancellationToken) : "",
                            VATTCHNAMEID = VATTCHNAMEID != null ? await trimForbidden(VATTCHNAMEID[j], cancellationToken) : "",
                            VSEQ = VSEQ != null ? VSEQ[j] : "",
                            VFLAG = VFLAG != null ? VFLAG[j] : "",
                            VTYPE = VTYPE != null ? VTYPE[j] : "",
                            VFILE = VATTCHNAMEONLY != null ? await trimForbidden(VATTCHNAMEONLY[j], cancellationToken) : "",

                            VPATH = VPATH != null ? VPATH[j] : "",
                        });
                    }
                }

                string vwfguid = "";
                bool issend = false;
                string themetype = await _hrirscontext.GetThemeType(cancellationToken);
                string vval = await _context.GetAreaByNrp(Convert.ToInt64(request.nrp), cancellationToken);
                var getArea = await _hrirscontext.GetAreaIDByNrp(Convert.ToInt64(request.nrp), vval, cancellationToken);

                string areaId = getArea.VAREAID;
                string regno = vnoreg.Equals("") ? await _context.GetRegistrationNum(Convert.ToInt64(request.nrp), themetype, areaId, cancellationToken) : vnoreg;

                bool isAttSucceed = true;
                string AttSucceed = "";

                if (VATTCHNAMEID != null)
                {
                    string doctype = "";
                    int seqValue = 0;
                    string doctypeBef = string.Empty;
                    for (int a = 0; a < VATTCHNAMEID.Length; a++)
                    {

                        if (VTYPE[a] == "af") doctype = "AFTER";
                        else if (VTYPE[a] == "bf") doctype = "BEFORE";
                        else if (VTYPE[a] == "std") doctype = "STANDARD";

                        string ext = System.IO.Path.GetExtension(VATTCHNAMEID[a]);
                        string pathFolder = await _context.GetPathAttachment("1", cancellationToken);
                        string fullPath = "";

                        string regnoSplit = regno.Substring(0, 5);
                        if (!string.IsNullOrEmpty(ext) || !string.IsNullOrEmpty(VATTCHNAMEONLY[a]))
                        {
                            string trimName = await trimForbidden(VATTCHNAMEONLY[a], cancellationToken);
                            fullPath = Path.Combine(pathFolder, regnoSplit + "-" + request.nrp.ToString() + "-IPS-" + regnoSplit + "-" + doctype + "-" + DateTime.Now.ToFileTime().ToString() + "_" + trimName);


                            string userFolder = request.Zone + "_" + _encryptionHelper.MD5Hash(request.domain + "\\" + request.userName);
                            string fileId = "";
                            string path = "";
                            string filePathTemp = await _hrirscontext.GetUploadedFilePath(userFolder, request.userName, request.domain, request.Zone, fileId, VATTCHNAMEID[a], cancellationToken);

                            if (VFLAG[a].Equals("IN"))
                            {
                                if (File.Exists(filePathTemp))
                                {
                                    try
                                    {
                                        Byte[] fileAttachment = File.ReadAllBytes(filePathTemp);
                                        File.WriteAllBytes(fullPath, fileAttachment);
                                    }
                                    catch (Exception ex)
                                    {

                                        result = "Failed to save data. Error : " + ex.Message;
                                        return result;
                                    }

                                }
                            }
                            else if (VFLAG[a].Equals("DEL"))
                            {
                                try
                                {
                                    File.Delete(Path.Combine(pathFolder, VATTCHNAMEID[a]));
                                }
                                catch (Exception ex)
                                {

                                    result = "Gagal menghapus dokumen. Dokumen tidak ditemukan / akses error.";
                                    return result;
                                }
                               
                            }
                            else if (VFLAG[a].Equals("UPDEL"))
                            {
                                try
                                {
                                    File.Delete(Path.Combine(pathFolder, VATTCHNAMEID[a]));
                                }
                                catch (Exception ex)
                                {

                                  
                                }
                              


                                if (File.Exists(filePathTemp))
                                {
                                    try
                                    {
                                        byte[] fileAttachment = File.ReadAllBytes(filePathTemp);
                                        File.WriteAllBytes(fullPath, fileAttachment);
                                    }
                                    catch (Exception ex)
                                    {

                                        result = "Gagal mengupload semua file. Error : " + ex.Message;
                                    }
                                   
                                }

                            }


                        }


                    }
                }

                if (isAttSucceed)
                {
                    if (v_action.Equals("submit"))
                    {

                        string vwfguid_input = myguid ?? "";

                        var executor = new WfExecutor(_configuration)
                        {
                            DisplayName = request.displayName,
                            Domain = request.domain,
                            Email = request.Email,
                            IpAddress = request.IpAddress,
                            Nrp = request.nrp,
                            Username = request.userName,
                            Zone = request.Zone
                        };

                        //var workflowVariables = new List<WfVariableRequest>();
                        //workflowVariables.Add(new WfVariableRequest() { VariableId = "@initTaskId", VariableValue = "task1" });

                        // 09042019 - Penambahan Pengecekan Delegasi Untuk Approval Workflow
                        string vnrpdelegasi = "";
                        long _inrp = 0;
                        string vnrpatasan = await _context.GetNrpAtasan(request.nrp, Convert.ToInt64(request.nrp), cancellationToken);
                        string vnrpfinal = "";
                        string cntdlgt = await _IItwfscontext.CekDelegasiApproval(vnrpatasan, cancellationToken);
                        //Logger.WriteLog("#DLGT COUNT : " + Convert.ToInt32(cntdlgt));
                        if (Convert.ToInt32(cntdlgt) > 0)
                        {
                            vnrpdelegasi = await _iitb2EInnovationContext.GetNRPDelegasi(vnrpatasan, cancellationToken);
                            vnrpfinal = vnrpdelegasi;
                        }
                        else
                        {
                            vnrpfinal = vnrpatasan;
                        }

                        string vuseridatasan = await _iitb2EInnovationContext.GetUserIdByNrp(vnrpfinal, cancellationToken);


                        //inisiasi workflow

                        //workflowVariables.Add(new WfVariableRequest() { VariableId = "@thema", VariableValue = vtema });
                        //workflowVariables.Add(new WfVariableRequest() { VariableId = "@ahmhrirs021_task1", VariableValue = vuseridatasan });

                        var picarea = await _hrirscontext.GetArea(varea, cancellationToken);

                        int i = 0;

                        //inisiasi workflow
                        //foreach (var pic in picarea)
                        //{
                        //    //if(!pic.Contains("AHMIC"))
                        //    //{
                        //    //wf.SetVariable ("@ahmhrirs021_pic" + i, pic.ToString ());
                        //    workflowVariables.Add(new WfVariableRequest() { VariableId = "@ahmhrirs021_pic" + i, VariableValue = pic.ToString() });
                        //    //}
                        //    i++;
                        //}

                        var vwfguid1 = await _workflow.RegisterWorkflow(new AHM.Domain.Workflow.Entities.WfRequest()
                        {
                            ApplicationId = _vappid,
                            FormId = _vformid,
                            DocumentId = "",
                            Description = "Mobile Attendance Planning",
                            WorkflowId = _workflowId,
                            WorkflowGuid = string.Empty,
                            Variables = new List<WfVariableRequest>()
                    {
                        new WfVariableRequest(){VariableId = "@thema",VariableValue = vtema},
                        new WfVariableRequest(){VariableId = "@ahmhrirs021_task1",VariableValue = vuseridatasan},
                        new WfVariableRequest(){VariableId = "@ahmhrirs021_pic",VariableValue = picarea[0].ToString()}

                    }

                        }, executor);

                        await _workflow.StartWorkflow(vwfguid1, executor);



                        // ini start workflow


                    }

                    decimal darea = 0, dnid = 0;
                    decimal.TryParse(varea, out darea);
                    decimal.TryParse(vnid, out dnid);

                    AHMHRIRS021_InfoSect infoSections = await _context.GetInfoSectionByNrp("", 0, Convert.ToInt64(request.nrp), cancellationToken);
                    AHMHRIRS021_IPDATA dataip = new AHMHRIRS021_IPDATA()
                    {
                        NID = dnid,
                        VNODOC = regno,
                        VTEMAIP = vtema,
                        VNRP = vnrp,
                        VNAME = infoSections.VNAMA,
                        VAREA = varea,
                        NAREA = darea,
                        VPHONE = vnoekst,
                        VDIREKTORAT = infoSections.DIR_ID,
                        VDIVISI = infoSections.DIV_ID,
                        VDEPARTMENT = infoSections.DEPT_ID,
                        VSUBDEPT = infoSections.SUBDEPT_ID,
                        VSECTION = infoSections.SEC_ID,
                        VPROCESS = vproses,
                        VIMPACT = vakibat,
                        VQUALITY = vquality,
                        VCOST = vcost,
                        VDELIVERY = vdeliv,
                        VSAFETY = vsafety,
                        VMORAL = vmoral,
                        VSTANDARD = vstandar,
                        NGOLONGAN = Convert.ToByte(vGolongan)
                    };

                    if (string.IsNullOrEmpty(vwfguid))
                    {
                        vwfguid = myguid;
                    }

                    if ((v_action.Equals("draft") || v_action.Equals("submit")) && vnoreg.Equals(""))
                    {
                        //Logger.WriteLog(vwfguid + " => simpan biasa " + AjaxResponse.SerializeObject(dataip));
                        string employeeStatus = await _context.GetStatusByNrp(request.nrp, cancellationToken);
                        _hrirscontext.InserDataHeaderDetail(employeeStatus, dataip, listAtt, issend, vwfguid, Convert.ToInt64(request.nrp), cancellationToken);

                        _hrirscontext.UpdateAttachmentId(dataip, listAtt, issend, cancellationToken);
                    }
                    else if (!vnoreg.Equals(""))
                    {
                        // Logger.WriteLog(vwfguid + " => Kirim Ke Atasan " + AjaxResponse.SerializeObject(dataip));
                        string lastTaskId = await _IItwfscontext.GetLastTaskid(vwfguid, cancellationToken);
                        _hrirscontext.UpdateDataHeaderDetail(lastTaskId, dataip, listAtt, issend, vwfguid, Convert.ToInt64(request.nrp), cancellationToken);

                        _hrirscontext.UpdateAttachmentId(dataip, listAtt, issend, cancellationToken);
                    }

                    result = "Success";

                }
                else
                {
                    result = "Lampiran gagal disimpan.";
                    return result;
                }


                return result;

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

        }




    }
}
